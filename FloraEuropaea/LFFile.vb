Imports System
Imports System.IO
Imports LfUtilities

' Class for simple file handling
Public Class LFFile

    Private fname As String
    Private hdl As Short
    Private mvOpenFlag As Boolean
    Private mvLine As String
    Private mv1stLine As String
    Private mv2ndLine As String
    Private mvLineNR As Integer
    Private mvEOF As Boolean

    Public ReadOnly Property Filename() As String
        Get
            Filename = fname
        End Get
    End Property

    Public ReadOnly Property fEOF() As Boolean
        Get
            fEOF = mvEOF
        End Get
    End Property

    Public ReadOnly Property GetHdl() As Short
        Get
            If hdl = 0 Then
                hdl = FreeFile()
            End If
            GetHdl = hdl
        End Get
    End Property

    Public ReadOnly Property OpenFlag() As Boolean
        Get
            OpenFlag = mvOpenFlag
        End Get
    End Property

    Public ReadOnly Property CurrentLine() As String
        Get
            CurrentLine = CInt(mvLine)
        End Get
    End Property

    Public ReadOnly Property FirstLine() As String
        Get
            FirstLine = CInt(mv1stLine)
        End Get
    End Property

    Public ReadOnly Property SecondLine() As String
        Get
            SecondLine = CInt(mv2ndLine)
        End Get
    End Property

    ' open text file for write access
    Sub openfileforwrite(ByRef pt As String, Optional ByRef fn As String = "")
        hdl = FreeFile()
        If Not Util.IsEmpty(fn) Then
            fname = lf_filename(pt, fn)
        Else
            fname = pt
        End If
        lf_delete(fname)
        FileOpen(hdl, fname, OpenMode.Output)
        mvOpenFlag = True
        mvLineNR = 0
        mvLine = ""
        mv1stLine = ""
        mv2ndLine = ""
        mvEOF = False
    End Sub

    ' open text file for read access
    Sub openfileforread(ByRef pt As String, Optional ByRef fn As String = "")
        hdl = FreeFile()
        If Not Util.IsEmpty(fn) Then
            fname = lf_filename(pt, fn)
        Else
            fname = pt
        End If

        FileOpen(hdl, fname, OpenMode.Input)

        mvOpenFlag = True
        mvLineNR = 0
        mvLine = ""
        mv1stLine = ""
        mv2ndLine = ""
        mvEOF = False

    End Sub

    ' close active file
    Sub closefile()
        FileClose(hdl)
        hdl = 0
        mvOpenFlag = False
        mvEOF = False
    End Sub

    ' write line into active file
    Sub putline(ByRef line As String)

        PrintLine(hdl, line)

        mvLineNR = mvLineNR + 1

        If mvLineNR = 1 Then
            mv1stLine = line
        ElseIf mvLineNR = 2 Then
            mv2ndLine = line
        End If
        mvLine = line

    End Sub

    ' read line from active file
    Function getline() As String

        If mvEOF Then
            getline = "$$$"
            Exit Function
        End If

        Try
            mvLine = LineInput(hdl)
            mvLineNR = mvLineNR + 1

            If mvLineNR = 1 Then
                mv1stLine = mvLine
            ElseIf mvLineNR = 2 Then
                mv2ndLine = mvLine
            End If
            mvEOF = EOF(hdl)
        Catch ex As Exception
            mvLine = ""
            mvEOF = True
        End Try

        getline = mvLine

    End Function

End Class
