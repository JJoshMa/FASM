module Elf.Header.FileHeader

open System

type EIClass =
    | ElfClassNone = 0x00uy
    | ElfClass32 = 0x01uy
    | ElfClass64 = 0x02uy

(* 
    ElfDataNone: 无效数据编码
    ElfData2LSB: 小端
    ElfData2MSB: 大端   
*)
type EIData =
    | ElfDataNone = 0x00uy
    | ElfData2LSB = 0x01uy
    | ElfData2MSB = 0x02uy

type EIVersion =
    | Default = 0x01uy

type EIAbiVersion =
    | None = 0x00uy

type EIPad(bytes: ReadOnlySpan<byte>) =
    new() =
        let bytes = [| 0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy |]
        EIPad(ReadOnlySpan(bytes))

type EType = 
    | Executable = 0x02s

type EMachine = 
    | X86 = 0x03s
    | Amd64 = 0x3Es

type EVersion = 
    | Default = 0x01s

(*
    虚拟内存地址偏移量程序的入口
    32位 0x08048000
    64位 0x400000
        x86-64架构的Linux系统中，可执行文件的默认加载基址通常是0x400000.
        这是因为ELF标准规定了64位程序的最低有效地址是0x400000
        gcc: 通常使用 0x1040 这个地址
        tiny cc: 使用 0x600000
*)
type EEntry = | Bytes of array<byte>

(*
    program headers begin
    程序头的开始位置
*)
type EPhoff = | Bytes of array<byte>

(* 
    section headers begin 
    段的开始位置
 *)
type EShoff = | Bytes of array<byte>


(*
    0x00000000 for x86
    Represents the EFlags register for x86 architecture.
*)
type EFlags = 
    { Bytes: byte array }

module EFlags =
    let init() : EFlags =
        { Bytes = [| 0x00uy; 0x00uy; 0x00uy; 0x00uy |] }
 
(*
    Represents the size of EEH in bytes.
*)
type EEHSize = 
    { Size: byte array }

module EEHSize =
    let getSys32() = 
        { Size = [| 0x34uy; 0x00uy |] }

    let getSys64() = 
        { Size = [| 0x40uy; 0x00uy |] }

type EPhentSize = 
    { Size: byte array }

module EPhentSize =
    let getSys32() = 
        { Size = [| 0x20uy; 0x00uy |] }

    let getSys64() = 
        { Size = [| 0x38uy; 0x00uy |] }

type EPhNum = 
    { Num:byte array }

module EPhNum =
    let init()  = 
        { Num = [|0x01uy; 0x00uy|] }

type EShentSize = 
    { Size: byte array }

type EShNum = 
    { Size: byte array }

module EShNum =
    let getSys32() = 
        { Size = [| 0x28uy; 0x00uy |] }

    let getSys64() = 
        { Size = [| 0x40uy; 0x00uy |] }

type EShstrndx = 
    { Index: byte array }


type FileHeader = 
    { EiMag0: byte
      EiMag1: byte 
      EiMag2: byte 
      EiMag3: byte 
      mutable EIClass: EIClass option  
      mutable EIData: EIData option
      mutable EIVersion: EIVersion option
      mutable EIAbiVersion: EIAbiVersion option
      mutable EIPad: EIPad option
      mutable EType: EType option
      mutable EEntry: EEntry option
      mutable EPhoff: EPhoff option
      mutable EFlags: EFlags option
      mutable EEHSize: EEHSize option
      mutable EPhentSize: EPhentSize option
      mutable EPhNum: EPhNum option
      mutable EShentSize: EShentSize option
      mutable EShNum: EShNum option
      mutable EShstrndx: EShstrndx option }
    
    static member make (?eiClass,?eiData,?eiVersion,?eiAbiVersion,?eiPad,?eType,
                        ?eEntry,?ePhoff,?eFlags,?eEhSize,?ePhentSize,?ePhNum,
                        ?eShentSize,?eShNum,?eShstrndx) = 
        {  EiMag0 = 0x7Fuy
           EiMag1 = 0x45uy
           EiMag2 = 0x4Cuy
           EiMag3 = 0x46uy
           EIClass = eiClass
           EIData = eiData
           EIVersion = eiVersion
           EIAbiVersion = eiAbiVersion
           EIPad = eiPad
           EType = eType
           EEntry = eEntry
           EPhoff = ePhoff
           EFlags = eFlags
           EEHSize = eEhSize
           EPhentSize = ePhentSize
           EPhNum = ePhNum
           EShentSize = eShentSize
           EShNum = eShNum
           EShstrndx = eShstrndx }
