Imports System.IO
Imports LfUtilities

<Serializable()> _
Public Class GlossaryElement
    Private mterm As String
    Private mdesc As String

    Public Property Term() As String
        Get
            Term = mterm
        End Get
        Set(ByVal Value As String)
            mterm = Value
        End Set
    End Property

    Public Property Description() As String
        Get
            Description = mdesc
        End Get
        Set(ByVal Value As String)
            mdesc = Value
        End Set
    End Property

    Public Sub New(ByVal term As String, ByVal description As String)
        Me.Term = term
        Me.Description = description
    End Sub
End Class

<Serializable()> _
Public Class Glossary
    Private bytes() As Byte
    Private text As String
    Private gl As New List(Of GlossaryElement)
    Private glfilt As New List(Of GlossaryElement)
    Private sresult(,) As Long
    Private filteronflag As Boolean
    Private Shared dataPath As String = Path.Combine(Application.StartupPath, "1.dat")
    Private filt As String = ""

    Public Property Filter() As String
        Get
            Return filt
        End Get
        Set(ByVal value As String)
            If value <> filt Then
                filt = value
                setFilter(filt)
            End If
        End Set
    End Property

    Public Property GelProperty As List(Of GlossaryElement)
        Get
            Return gl
        End Get
        Set(value As List(Of GlossaryElement))
            gl = value
        End Set
    End Property

    Public Sub New()

    End Sub

    Public Function IsFilled() As Boolean
        If gl Is Nothing Then
            Return False
        Else
            If gl.Count > 500 Then
                Return True
            Else
                Return False
            End If
        End If
    End Function

    Public Sub LoadData()
        Dim pos1 As Long, pos2 As Long, pos3 As Long, startpos As Long
        Dim t As String, d As String

        Dim wit As Long = 0, x As String = ""

        Me.bytes = File.ReadAllBytes(sett.FloraExe)
        text = System.Text.Encoding.Default.GetString(bytes)
        Dim kf3 As String = "\par \plain\lang2057\f3\fs28\b"
        Dim kf2 As String = "\par \plain\lang2057\f2\fs28\b"
        Dim kf3new As String = "{f3}"
        text = text.Replace(kf3, kf3new).Replace(kf2, kf3new)
        Dim kf3len = kf3.Length
        Dim kf3newlen = kf3new.Length

        pos1 = lf_Instr(text, kf3new, startpos)
        If pos1 = 0 Then
            pos1 = text.IndexOf(kf3new, CInt(startpos))
            text = text.Substring(pos1)
        End If
        pos2 = text.LastIndexOf(kf3new)
        text = text.Left(pos2 + 500)
        startpos = 1
        Do
            pos1 = lf_Instr(text, kf3new, startpos)
            If pos1 > 0 Then
                startpos = pos1 + kf3newlen + 1
                pos2 = lf_Instr(text, "\tab", startpos)
                startpos = pos2 + 5
                If pos2 > pos1 Then
                    t = Trim(lf_GetSubstr(text, pos1 + kf3newlen + 1, pos2 - 1))
                    startpos = pos2 + kf3newlen + 1
                    pos3 = lf_Instr(text, "\tab", startpos)
                    If pos3 = 0 Then
                        pos3 = text.Length + 1
                    End If
                    d = Trim(lf_GetSubstr(text, pos2 + kf3newlen, pos3 - 1))
                    startpos = pos3 + 5
                    Dim el As New GlossaryElement(t, d)
                    gl.Add(el)
                End If
            End If
            If pos1 = 0 Then Exit Do
            wit = wit + 1
            If wit > 2000 Then
                Exit Do
            End If
            If startpos > text.Length Then
                Exit Do
            End If
        Loop
        glfilt = gl
        filt = ""
        filteronflag = False

    End Sub

    Public Sub setfilterOn(ByVal flt As String)
        If Not Util.IsEmpty(flt) Then
            setFilter(flt)
            filteronflag = True
        End If
    End Sub

    Public Sub setfilterOn()
        If Not Util.IsEmpty(filt) Then
            filteronflag = True
        End If
    End Sub

    Public Sub setfilterOff()
        glfilt = gl
        filteronflag = False
    End Sub

    Public Sub setFilter(ByVal f As String)
        glfilt = New List(Of GlossaryElement)()
        Dim el As GlossaryElement
        Dim p As String = "^$$F$.*$"
        filt = f
        p = Replace(p, "$$F$", f)
        For Each el In gl
            If Util.Match(el.Term, p, True) Then
                glfilt.Add(el)
            End If
        Next
    End Sub

    Public Function Search(ByVal s As String) As GlossaryElement
        Dim foundflag As Boolean = False
        Dim idx As Long
        Dim i As Long
        Dim offs As Long
        Dim w As String

        If Util.IsEmpty(s) Then
            Return Nothing
        End If

        If Searchterm(s) Then
            foundflag = True
        Else
            If (s.EndsWith("ia")) Then
                w = Util.Left(s, s.Length - 2) & "ium"
                If Searchterm(w) Then
                    foundflag = True
                End If
            End If
            If Not foundflag Then
                If Searchterm(Util.Left(s, s.Length - 1)) Then
                    foundflag = True
                Else
                    If Searchtdesc(s) Then
                        foundflag = True
                    End If
                End If
            End If
        End If
        idx = -1
        If foundflag Then
            offs = 1000
            For i = 0 To 20
                If Me.sresult(i, 1) > 0 Then
                    If Me.sresult(i, 1) < offs Then
                        idx = Me.sresult(i, 0)
                        offs = Me.sresult(i, 1)
                    End If
                End If
            Next
            If idx > -1 Then
                Return gl(idx)
            End If
        End If
        Return Nothing
    End Function

    Public Function Searchterm(ByVal t As String) As Boolean
        Dim i As Long, wst As String, we As GlossaryElement, wt As String
        Dim foundflag As Boolean = False
        Dim sr(20, 1) As Long, si As Long, x As Long
        wst = t.ToUpper
        For i = 0 To gl.Count - 1
            we = gl(i)
            wt = we.Term.ToUpper
            x = lf_Instr(wt, wst)
            If x > 0 Then
                foundflag = True
                If si <= 20 Then
                    sr(si, 0) = i
                    sr(si, 1) = x
                    si = si + 1
                End If
            End If
            If si > 20 Or x = 1 Then
                Exit For
            End If
        Next
        Me.sresult = sr
        Return foundflag
    End Function

    Public Function Searchterm2(ByVal t As String) As String
        Dim i As Long, wst As String, we As GlossaryElement, wt As String
        Dim sr As String = "", x As Long
        wst = t.ToUpper
        For i = 0 To gl.Count - 1
            we = gl(i)
            wt = we.Term.ToUpper
            x = lf_Instr(wt, wst)
            If x > 0 Then
                sr = we.Description
            End If
        Next
        Return sr
    End Function

    Public Function Searchtdesc(ByVal d As String) As Boolean
        Dim i As Long, wsd As String, we As GlossaryElement, wd As String
        Dim foundflag As Boolean = False
        Dim sr(20, 1) As Long, x As Long
        wsd = d.ToUpper
        For i = 0 To gl.Count - 1
            we = gl(i)
            wd = we.Description.ToUpper
            x = lf_Instr(wd, wsd)
            If x > 0 Then
                foundflag = True
                sr(0, 0) = i
                sr(0, 1) = x
                Exit For
            End If
        Next
        Me.sresult = sr
        Return foundflag
    End Function

    Public Sub Fillcontrol(ByRef lb As ListBox)
        Dim el As GlossaryElement
        lb.Items.Clear()
        If Me.filteronflag Then
            For Each el In glfilt
                lb.Items.Add(el.Term)
            Next
        Else
            For Each el In gl
                lb.Items.Add(el.Term)
            Next
        End If
    End Sub

    Public Sub Save()
        ' Aufrufen der generischen Serialize-Funktion für 
        ' die Klasse Kontoverwaltung
        Serializer.JsonSerialize(Of List(Of GlossaryElement))(True, dataPath, Me.GelProperty)
    End Sub

    Public Shared Function Load() As List(Of GlossaryElement)
        ' Dadurch, dass die Klasse "Sub New()" aufweist, kann die 
        ' Serializer-Klasse, wenn keine Datei vorhanden ist, 
        ' eine DefaultInstance zurückgeben.        
        Return Serializer.DeJsonSerialize(Of List(Of GlossaryElement))(True, dataPath)
    End Function

End Class
