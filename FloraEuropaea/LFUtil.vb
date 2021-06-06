Imports System.Text.RegularExpressions
Imports System.Math
Imports System.Reflection
Imports System.IO
Imports LfUtilities

Module LFUtil


    ' show teststring in message box
    Sub lf_showTestString(ByVal TestString As String)
        MsgBox("/" + TestString + "/")
    End Sub

    ' show string array in message box
    Sub lf_showStringArray(ByVal tit As String, ByVal sa() As String)
        Dim i As Long
        Dim x As String
        If IsNothing(tit) Then
            tit = ""
        End If
        x = tit & ": " & Chr(10)
        If Not IsNothing(sa) Then
            For i = LBound(sa) To UBound(sa)
                x = x & sa(i) & Chr(10)
            Next
        End If
        MsgBox(x)
    End Sub

    ' Return substr starting at position startpos and ending at endpos
    Function lf_GetSubstr(ByVal s As String, ByVal startpos As Long, Optional ByVal endpos As Long = -1) As String
        If endpos < 1 Then
            lf_GetSubstr = Mid(s, startpos)
        ElseIf endpos < startpos Then
            lf_GetSubstr = ""
        Else
            lf_GetSubstr = Mid(s, startpos, endpos - startpos + 1)
        End If
    End Function

    ' Return substr starting at position startpos of length ln
    Function lf_Substr(ByVal s As String, ByVal startpos As Long, Optional ByVal ln As Long = -1) As String

        If Util.IsEmpty(s) Then
            Return ""
        End If

        If ln >= 0 Then
            lf_Substr = Mid(s, startpos, ln)
        Else
            lf_Substr = Mid(s, startpos)
        End If

    End Function

    ' Simulates some features of instr of oracle plsql
    ' s: input string, ss: searchstring, startpos: start position,
    ' nocc: number of occurrence to search for
    Function lf_Instr(ByVal s As String, ByVal ss As String, Optional ByVal startpos As Long = 1, Optional ByVal nocc As Long = 1,
        Optional ByVal c As Microsoft.VisualBasic.CompareMethod = CompareMethod.Text) As Long

        Dim hpos As Long, nc As Long, revflag As Boolean = False
        hpos = startpos
        nc = 0

        If Util.IsEmpty(s) Or Util.IsEmpty(ss) Then
            lf_Instr = 0
            Exit Function
        End If

        If startpos = 0 Then
            hpos = 1
            startpos = 1
        ElseIf startpos < 0 Then
            revflag = True
            startpos = Abs(startpos)
            If startpos = 1 Then
                startpos = -1
            End If
        End If

        If revflag Then
            If nocc = 1 Then
                lf_Instr = InStrRev(s, ss, startpos, c)
            Else
                hpos = startpos
                Do
                    hpos = InStrRev(s, ss, hpos, c)
                    nc = nc + 1
                Loop Until nc >= nocc Or hpos = 0
                lf_Instr = hpos
            End If
        Else
            If nocc = 1 Then
                lf_Instr = InStr(startpos, s, ss, c)
            Else
                hpos = startpos
                Do
                    hpos = InStr(hpos, s, ss, c)
                    nc = nc + 1
                Loop Until nc >= nocc Or hpos = 0
                lf_Instr = hpos
            End If
        End If

    End Function

    ' wie lf_Instr, gibt jedoch len(s) zurueck, falls nichts gefunden wird
    Function lf_InstrS(
        ByVal s As String,
        ByVal ss As String,
        Optional ByVal startpos As Integer = 1
        ) As Long

        If Util.IsEmpty(s) Or Util.IsEmpty(ss) Then
            lf_InstrS = 0
            Exit Function
        End If

        Dim vPos As Integer
        vPos = lf_Instr(s, ss, startpos)
        If vPos = 0 Then
            vPos = Len(s)
        End If
        Return vPos
    End Function

    ' Returns longest word in sentence
    ' s: sentence
    Function lf_getLongestWord(ByVal s As String) As String
        Dim w As String = s, ws As String, l As Long
        s = Util.Translate(s, ",;.()", "      ")
        s = Util.RemoveDblspaces(s)
        Dim a() As String = Split(s, " ")
        Dim maxl As Long = -1
        For Each ws In a
            l = ws.Trim.Length
            If l > maxl Then
                maxl = l
                w = ws.Trim
            End If
        Next
        Return w
    End Function

    ' Returns regular expression for tolerant search of s
    ' s: search expression
    Function lf_getRegExp(ByVal s As String) As String
        Dim tplrexp As String, rexp As String = ""
        If s.Contains(" ") Then
            tplrexp = "$$X$"
        Else
            tplrexp = "$$X$\s"
        End If
        Dim p As String = "[\s\.,():\-;?]*"
        rexp = Util.Translate(s, ".-,:()?", "          ")
        rexp = Util.Translate(rexp, "üöäÜÖÄéàèÉÀÈáéúí", "-----------------")
        rexp = Replace(rexp, "-", "\w")
        rexp = Util.RemoveDblspaces(rexp).Trim
        rexp = Replace(rexp, " ", p)
        rexp = Replace(tplrexp, "$$X$", rexp)
        Return rexp
    End Function


    Function lf_CleanPathName(ByVal p1 As String, Optional ByVal p2 As String = "", Optional ByVal p3 As String = "", Optional ByVal p4 As String = "") As String
        lf_CleanPathName = p1 & "\" & p2 & "\" & p3 & "\" & p4 & "\"
        lf_CleanPathName = Replace(lf_CleanPathName, "//", "\")
        lf_CleanPathName = Replace(lf_CleanPathName, "/\", "\")
        lf_CleanPathName = Replace(lf_CleanPathName, "\/", "\")
        lf_CleanPathName = Replace(lf_CleanPathName, "\\", "\")
        lf_CleanPathName = Replace(lf_CleanPathName, "\\", "\")
        lf_CleanPathName = Replace(lf_CleanPathName, "\\", "\")
    End Function

    '
    ' Name:  lf_MKL  (FUNCTION)  (C) foerderer
    ' Zweck:    Bestimmt die Position der Schlussklammer eines
    '           Klammerausdrucks
    ' Hin:      w   : String mit Klammerausdruck, der zu untersuchen ist
    '           P1  : Position der Anfangsklammer in w
    ' Zurück:   Position der Schlussklammer zu Anfangsklammer
    '
    Function lf_MKL(ByVal w As String, ByVal p1 As Long) As Long

        If Util.IsEmpty(w) Then
            lf_MKL = 0
            Exit Function
        End If

        Dim hp As Long, kl As Long, ind As Long, h1 As String
        hp = p1 + 1
        kl = 1
        ind = 0
        Do
            hp = hp + 1
            h1 = Mid(w, hp, 1)
            If h1 = "(" Then
                kl = kl + 1
            End If
            If h1 = ")" Then
                kl = kl - 1
            End If
        Loop Until (kl = 0 And h1 = ")") Or ind > 10000
        lf_MKL = hp
    End Function

    ' swap content of s1 and s2
    Sub lf_StrSwap(ByRef s1 As String, ByRef s2 As String)
        Dim w As String
        w = s1
        s1 = s2
        s2 = w
    End Sub

    ' simuliert plsql IN() mit String
    Function lf_In(ByVal pkey As String, ByVal p1 As String, Optional ByVal p2 As String = "", Optional ByVal p3 As String = "", Optional ByVal p4 As String = "", Optional ByVal p5 As String = "") As Boolean

        lf_In = False

        If Util.IsEmpty(pkey) Then
            Exit Function
        End If

        If pkey = p1 Or pkey = p2 Or pkey = p3 Or pkey = p4 Or pkey = p5 Then
            lf_In = True
            Exit Function
        End If

    End Function

    '  Groesster Wert vom Typ long
    Function lf_maxL(ByVal v1 As Long, ByVal v2 As Long, Optional ByVal v3 As Long = -9 * 10 ^ 10, Optional ByVal v4 As Long = -9 * 10 ^ 10) As Long
        lf_maxL = v1
        If lf_maxL < v2 Then
            lf_maxL = v2
        End If
        If lf_maxL < v3 Then
            lf_maxL = v3
        End If
        If lf_maxL < v4 Then
            lf_maxL = v4
        End If
    End Function

    '  Kleinster Wert vom Typ long
    Function lf_minL(ByVal v1 As Long, ByVal v2 As Long, Optional ByVal v3 As Long = 9 * 10 ^ 10, Optional ByVal v4 As Long = 9 * 10 ^ 10) As Long
        lf_minL = v1
        If lf_minL > v2 Then
            lf_minL = v2
        End If
        If lf_minL > v3 Then
            lf_minL = v3
        End If
        If lf_minL > v4 Then
            lf_minL = v4
        End If
    End Function

    Sub OpenUrl(u As String)
        Process.Start(New ProcessStartInfo(u) With {
            .UseShellExecute = True
        })

    End Sub






































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































    ' Simulates Oracle NVL wirh String
    Function lf_NVL(ByVal s1 As String, ByVal s2 As String) As String
        If Util.IsEmpty(s1) Then
            Return s2
        Else
            Return s1
        End If
    End Function

    ' Returns True if p_Position is within p_BlockStartTag/p_BlockEndTag block
    Function lf_IsWithin(
        ByVal pSource As String,
        ByVal pPosition As Integer,
        ByVal pBlockStartTag As String,
        ByVal pBlockEndTag As String
       ) As Boolean

        Dim vReturnValue As Boolean = False
        Dim vStartBlockPosition As Integer
        Dim vEndBlockPosition As Integer
        Dim vWrk As String

        If Util.IsEmpty(pSource) Or Util.IsEmpty(pBlockStartTag) Or Util.IsEmpty(pBlockEndTag) Then
            vReturnValue = False
        Else

            vWrk = Util.Left(pSource, pPosition)
            vStartBlockPosition = lf_Instr(vWrk, pBlockStartTag, -1)
            vEndBlockPosition = lf_Instr(vWrk, pBlockEndTag, -1)
            If vStartBlockPosition > 0 And vEndBlockPosition < vStartBlockPosition Then
                vReturnValue = True
            Else
                vStartBlockPosition = 0
            End If
        End If
        pPosition = vStartBlockPosition + 5
        Return vReturnValue
    End Function

    ' Returns cmdKey and cmdValue of Cmd
    Sub lf_getCmdInfo(ByVal cmd As String, ByRef cmdkey As String, ByRef cmdvalue As String)
        Dim pos As Long

        cmdkey = ""
        cmdvalue = ""

        If Util.IsEmpty(cmd) Then
            Exit Sub
        End If

        pos = lf_Instr(cmd, "=")

        If pos > 0 Then
            cmdkey = Trim(Left(cmd, pos - 1))
            cmdvalue = Trim(lf_Substr(cmd, pos + 1))
        End If

    End Sub

    ' Returns true if all characters of string are upper case, else false
    Function lf_AllUpperCase(ByVal stringToCheck As String) As Boolean
        lf_AllUpperCase = StrComp(stringToCheck, UCase(stringToCheck),
           vbBinaryCompare) = 0
    End Function

    ' Returns true if all characters of string are lower case, else false
    Function lf_AllLowerCase(ByVal stringToCheck As String) As Boolean
        lf_AllLowerCase = StrComp(stringToCheck, LCase(stringToCheck),
           vbBinaryCompare) = 0
    End Function

    ' Returns true if all characters of string are upper case, else false
    Function lf_FirstUpperCase(ByVal stringToCheck As String) As Boolean
        lf_FirstUpperCase = Util.AllUpperCase(UTIL.Left(stringToCheck.Trim, 1))
    End Function

    ' Returns true if all characters of string are lower case, else false
    Function lf_FirstLowerCase(ByVal stringToCheck As String) As Boolean
        lf_FirstLowerCase = Util.AllLowerCase(Util.Left(stringToCheck.Trim, 1))
    End Function

    ' Verzeichnis von aktueller Application ermitteln
    Public Function lf_GetApplicationFolderName() As String
        ' FileInfo-Objekt für die Datei erzeugen, die die Eintritts-Assembly speichert
        Dim fi As FileInfo = New FileInfo(Assembly.GetEntryAssembly().Location)
        ' Den Pfad des Ordners der Datei zurckgeben
        Return fi.DirectoryName
    End Function

    Public Function lf_DirName(ByVal f As String) As String
        Dim r As String = ""
        r = Path.GetDirectoryName(f)
        Return r
    End Function

End Module
