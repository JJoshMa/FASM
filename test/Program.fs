// For more information see https://aka.ms/fsharp-console-apps

module test = 
    open Elf
    open System.IO

    let writeInt16ToFile (filePath: string) (value: int16) =
        // 使用 FileStream 和 BinaryWriter 来写入 int16 值
        use fs = new FileStream(filePath, FileMode.Create, FileAccess.Write)
        use writer = new BinaryWriter(fs)
        
        writer.Write(value)

let main () =
    let filePath = "output.bin"  // 文件路径
    let valueToWrite  = int16 12345  // 要写入的 int16 值
    
    test.writeInt16ToFile filePath valueToWrite
    printfn "Successfully wrote %d to %s" valueToWrite filePath

main ()