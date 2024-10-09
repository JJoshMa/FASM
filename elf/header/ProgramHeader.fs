module Elf.Header.ProgramHeader

open System

(*
    1 (0x1): X - Executable
    2 (0x2): W - Writable
    4 (0x4): R - Readable
*)
type PFlags =
    | X = 0x1u
    | W = 0x2u
    | R = 0x4u
    | XW = 0x3u
    | XR = 0x5u
    | WR = 0x6u
    | ALL = 0x7u

type PAlign =
    | X86 = 0x1000UL

type ProgramHeader =
    { PType: uint32 option
      mutable POffset: uint32 option
      mutable PVAddr: uint32 option
      mutable PAddr: uint32 option
      mutable PFileSize: uint32 option
      mutable PMemSize: uint32 option
      mutable PFlags: PFlags option
      PAlign: PAlign option }

    member this.Generator() =
        let result = ResizeArray<byte>()
        result.AddRange <| BitConverter.GetBytes(this.PType.Value)
        result.AddRange <| BitConverter.GetBytes(this.POffset.Value)
        result.AddRange <| BitConverter.GetBytes(this.PVAddr.Value)
        result.AddRange <| BitConverter.GetBytes(this.PAddr.Value)
        result.AddRange <| BitConverter.GetBytes(this.PFileSize.Value)
        result.AddRange <| BitConverter.GetBytes(this.PMemSize.Value)
        result.AddRange <| BitConverter.GetBytes(uint32 this.PFlags.Value)
        result.AddRange <| BitConverter.GetBytes(uint32 this.PAlign.Value)
        result

    static member size() = 0x20u

    static member make(?pType, ?pOffset, ?pVAddr, ?pAddr, ?pFileSize, ?pMemSize, ?pFlags, ?pAlign) =
        { PType = pType
          POffset = pOffset
          PVAddr = pVAddr
          PAddr = pAddr
          PFileSize = pFileSize
          PMemSize = pMemSize
          PFlags = pFlags
          PAlign = pAlign }
