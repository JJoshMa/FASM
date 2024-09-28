module Elf.Header.MagicHeader

open System

type EIClass =
    | ElfClassNone = 0x00uy
    | ElfClass32 = 0x01uy
    | ElfClass64 = 0x02uy

type EIData =
    // 无效数据编码
    | ElfDataNone = 0x00uy
    // 小端
    | ElfData2LSB = 0x01uy
    // 大端
    | ElfData2MSB = 0x02uy

type EIVersion =
    | Default = 0x01uy

type EIAbiVersion =
    | None = 0x00uy

type EIPad(bytes: ReadOnlySpan<byte>) =
    new() =
        let bytes = [| 0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy; 0x00uy |]
        EIPad(ReadOnlySpan(bytes))

type MagicHeader =
    struct
        val eiMag0: byte // 0x7f
        val eiMag1: byte // 'E'
        val eiMag2: byte // 'L'
        val eiMag3: byte // 'F'

        val eiClass: EIClass

        val eiData: EIData

        val eiVersion: EIVersion
        val eiOSAbi: EIAbiVersion

        val eiPad: EIPad

    end
