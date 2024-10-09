// For more information see https://aka.ms/fsharp-console-apps
open System.IO
open Elf.Header.FileHeader
open Elf.Header.ProgramHeader
open System

let bytesToHex (bytes: byte array) : string =
    bytes |> Array.map (fun b -> sprintf "%02X" b) |> String.concat " "

let byteToHex (bytes: byte) : string = sprintf "%02X" bytes

let debugger (bytes: ResizeArray<byte>) =
    let chunkSize = 4
    let length = bytes.Count

    for i in 0..chunkSize .. (length - 1) do
        let count = System.Math.Min(chunkSize, length - i)
        let chunk = bytes.GetRange(i, count)
        let hexValues = chunk |> Seq.map (fun b -> sprintf "%02X" b) |> String.concat " "
        printfn "%s" hexValues

let generatorElf (code: byte[]) =
    let fileHeader =
        FileHeader.make (
            eiClass = EIClass.ElfClass32,
            eiData = EIData.ElfData2LSB,
            eiVersion = EIVersion.Default,
            eiOsAbi = EIOsAbi.None,
            eiAbiVersion = EIAbiVersion.None,
            eiPad = EIPad(),
            eType = EType.Executable,
            eMachine = EMachine.X86,
            eVersion = EVersion.Default,
            eFlags = EFlags.init (),
            eEhSize = EEHSize.getSys32 (),
            ePhentSize = EPhentSize.getSys32 (),
            ePhNum = EPhNum.init (),
            eShentSize = EShentSize.getSys32 (),
            eShNum = EShNum.make [| 0x00uy; 0x00uy |],
            eShstrndx = EShstrndx.make [| 0x00uy; 0x00uy |]
        )

    let programmerHeader =
        ProgramHeader.make (pType = 0x1u, pFlags = PFlags.ALL, pAddr = 0x0u, pAlign = PAlign.X86)

    programmerHeader.POffset <- Some <| (ProgramHeader.size () + 0x34u)
    programmerHeader.PVAddr <- Some(0x08048000u + programmerHeader.POffset.Value)

    programmerHeader.PFileSize <- Some(uint32 code.Length)
    programmerHeader.PMemSize <- Some(uint32 code.Length)


    // Offset for programmers as dynamic
    fileHeader.EEntry <- Some <| (EEntry.Bytes <| BitConverter.GetBytes(programmerHeader.PVAddr.Value))

    // ELF Program Header Start
    // This might be the beginning of this header
    fileHeader.EPhoff <- Some <| EPhoff.Bytes [| 0x34uy; 0x00uy; 0x00uy; 0x00uy |]

    // ELF Section headers begin
    fileHeader.EShoff <- Some <| EShoff.Bytes [| 0x0uy; 0x00uy; 0x00uy; 0x00uy |]

    let f: ResizeArray<byte> = fileHeader.Generator()
    f.AddRange <| programmerHeader.Generator()
    f.AddRange <| code

    f

let main () =
    let code =
        [|
           // mov eax 0x01
           0xB8
           0x09
           0x00
           0x00
           0x00
           // add
           0x2D
           0x03
           0x00
           0x00
           0x00

           0xB9
           0x82
           0x80
           0x04
           0x08
 
           0x89
           0x01

           0xB8
           0x04
           0x00
           0x00
           0x00

           0xBB
           0x01
           0x00
           0x00
           0x00

           0xBA
           0x04
           0x00
           0x00
           0x00

           0xCD
           0x80

           0xB8
           0x01
           0x00
           0x00
           0x00

           0XBB
           0x00
           0x00
           0x00
           0x00

           0xCD
           0x80

           0x00
           0x00
           0x00
           0x00 |]

    let byteCode: byte[] = Array.map byte code

    let programmer = generatorElf byteCode
    debugger programmer

    // 0x86
    printfn $"size {programmer.Count}"

    let filePath = "test"

    let fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite)
    let writer = new BinaryWriter(fs)
    writer.Write(ReadOnlySpan(programmer.ToArray()))
    writer.Close()
    ()

main ()
