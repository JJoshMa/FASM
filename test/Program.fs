// For more information see https://aka.ms/fsharp-console-apps
open System.IO
open Elf.Header.FileHeader

let bytesToHex (bytes: byte array) : string =
    bytes |> Array.map (fun b -> sprintf "%02X" b) |> String.concat " "

let byteToHex (bytes: byte) : string =
    sprintf "%02X" bytes 


let main () =

    let fileHeader = FileHeader.make()



    let filePath = "test"
    let valueToWrite  = int16 0x34 

    let fs = new FileStream(filePath, FileMode.Create,FileAccess.ReadWrite)
    let writer = new BinaryWriter(fs)
    writer.Write valueToWrite
    
    fs.Seek(0L,SeekOrigin.Begin) |> ignore
    let reader = new BinaryReader(fs)
    let bytes = reader.ReadBytes(4)
    printfn $"{bytesToHex bytes}"
    

    reader.Close()
    fs.Close()

    File.Delete(filePath)
    // 4857 
    ()

main ()