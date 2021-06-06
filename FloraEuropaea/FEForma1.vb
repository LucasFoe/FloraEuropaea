Imports System.Text.RegularExpressions
Imports System.Diagnostics
Imports System.Windows.Forms
Imports LfUtilities

Public Class FEForma1

    ' Taxon selectted in lbIndex
    Private currentTaxon As String
    Private currentSpecies As String

    Private lastSettingStatusOk As Boolean = False
    Private lastTextStatusOK As Boolean = False
    Private lastDBStatusOK As Boolean = False
    Private lastSearchStatusOK As Boolean = False
    Private lastFloraExeStatusOK As Boolean = False

    ' Line Index of first Match in HighlightWords
    Private lFirstMatchPosition As Integer
    Private lockEvents As Boolean = False

    Private checkPrint As Integer

    Private startFilterFlag As Boolean = False
    Private speciesflt As String = ""

    Public Sub Reload()
        Dim e As EventArgs = Nothing
        FEForma1_Load(Me, e)
    End Sub

    Public Sub setmessage(ByVal l As String, ByVal m As String)
        Dim mtpl As String = "$$L$$$M$", msg As String
        msg = Replace(mtpl, "$$L$", l)
        msg = Replace(msg, "$$M$", m)
        ToolStripStatusLabel2.Text = msg.Trim
    End Sub

    Public Sub setmessage(ByVal m As String)
        setmessage("Status Message: ", m)
    End Sub

    Public Sub errmessage(ByVal m As String)
        setmessage("Error Message: ", m)
    End Sub

    Public Sub errmessage(ByRef e As Exception)
        setmessage("Error Message: ", e.Message)
    End Sub

    Public Sub resetmessage()
        ToolStripStatusLabel2.Text = ""
    End Sub

    Public Sub SetStartFlt(genusflt As String, speciesflt As String)
        Me.txtFilter.Text = genusflt
        Me.speciesflt = speciesflt
        Me.startFilterFlag = True
    End Sub

    Public Sub SetFlt()
        Dim s As String = txtFilter.Text.ToString.Trim
        resetmessage()
        Me.ckFilter.Checked = True
        If s.Length > 0 Then
            lbSpec.DataSource = Nothing
            Dim ftpl As String = "PNAME LIKE '$$X$%'", f As String
            f = Replace(ftpl, "$$X$", s)
            f = Replace(f, "%%", "%")
            f = Replace(f, "%%", "%")
            Try
                pi.setfilter(f)
            Catch ex As Exception
                errmessage(ex)
            End Try
            lbIndex.SelectedIndex = -1
        Else
            ResetFlt()
        End If

    End Sub

    Public Sub ResetFlt()
        resetmessage()
        Dim f As String = Nothing
        Try
            lbSpec.DataSource = Nothing
            pi.setfilter(f)
        Catch ex As Exception
            errmessage(ex)
        End Try
        lbIndex.SelectedIndex = -1
    End Sub

    Public Function HighlightWords(ByRef rtb As RichTextBox, ByVal sFindString As String, ByVal lColor As System.Drawing.Color) As Integer
        Dim lFoundPos As Integer        'Position of first character
        Dim lstartpos As Integer
        'of match
        Dim lFindLength As Long         'Length of string to find
        Dim lOriginalSelStart As Long
        Dim lOriginalSelLength As Long
        Dim iMatchCount As Integer      'Number of matches

        'Save the insertion points current location and length
        lOriginalSelStart = rtb.SelectionStart
        lOriginalSelLength = rtb.SelectionLength

        Dim rexp As String
        rexp = lf_getRegExp(sFindString)
        iMatchCount = HighlightWordsRegExp(rtb, rexp, lColor)
        If iMatchCount > 0 Then
            Return iMatchCount
        End If

        'Attempt to find the first match
        lFoundPos = rtb.Find(sFindString, 0, RichTextBoxFinds.NoHighlight)

        If lFoundPos < 0 Then
            rexp = lf_getRegExp(sFindString)
            iMatchCount = HighlightWordsRegExp(rtb, rexp, lColor)
            If iMatchCount > 0 Then
                Return iMatchCount
            End If
        End If

        If lFoundPos < 0 Then
            sFindString = lf_getLongestWord(sFindString)
            lFoundPos = rtb.Find(sFindString, 0, RichTextBoxFinds.NoHighlight)
        End If

        lFirstMatchPosition = lFoundPos

        'Cache the length of the string to find
        lFindLength = Len(sFindString)

        While lFoundPos > -1
            iMatchCount = iMatchCount + 1
            rtb.SelectionStart = lFoundPos
            'The SelLength property is set to 0 as
            'soon as you change SelStart
            rtb.SelectionLength = lFindLength + 1
            rtb.SelectionBackColor = lColor

            'Attempt to find the next match
            lstartpos = lFoundPos + lFindLength
            lFoundPos = rtb.Find(sFindString, lstartpos, RichTextBoxFinds.NoHighlight)
        End While

        'Restore the insertion point to its original
        'location and length
        rtb.SelectionStart = lOriginalSelStart
        rtb.SelectionLength = lOriginalSelLength

        'Return the number of matches
        Return iMatchCount

    End Function

    Public Function HighlightWordsRegExp(ByRef rtb As RichTextBox, ByVal p As String, ByVal lColor As System.Drawing.Color) As Integer
        Dim iMatchCount As Integer = 0
        Dim lOriginalSelStart As Long
        Dim lOriginalSelLength As Long
        Dim wtxt As String = RichTextBox1.Text
        wtxt = Util.Translate(wtxt, "üöäÜÖÄéàèÉÀÈáéúí", "uoaUOAeaeEAEaeui")

        'Save the insertion points current location and length
        lOriginalSelStart = rtb.SelectionStart
        lOriginalSelLength = rtb.SelectionLength

        Try
            Dim reg_exp As New Regex(p, RegexOptions.IgnoreCase Or RegexOptions.Multiline)
            Dim matches As MatchCollection
            matches = reg_exp.Matches(wtxt)

            lFirstMatchPosition = matches(0).Index
            For Each a_match As Match In matches
                RichTextBox1.Select(a_match.Index, a_match.Value.TrimEnd().Length)
                RichTextBox1.SelectionBackColor = lColor
                iMatchCount = iMatchCount + 1
            Next a_match
        Catch ex As Exception
            errmessage(ex.Message)
        End Try
        'Restore the insertion point to its original
        'location and length
        rtb.SelectionStart = lOriginalSelStart
        rtb.SelectionLength = lOriginalSelLength
        Return iMatchCount
    End Function

    Public Sub FindlbSpec(ByVal s As String)
        Dim w As String, ws As String = s.ToUpper, i As Long, drv As DataRowView
        resetmessage()
        lbSpec.SelectedIndex = -1
        Try
            Me.Cursor = Cursors.WaitCursor

            For i = 0 To lbSpec.Items.Count - 1
                drv = lbSpec.Items(i)
                w = drv("PNAME").ToString.ToUpper
                If lf_Instr(w, ws) > 0 Then
                    lbSpec.SelectedIndex = i
                    Exit For
                End If
            Next
            Me.Cursor = Cursors.Default

        Catch ex As Exception
            errmessage(ex.Message)
        End Try

    End Sub

    Public Sub LbIndexActionNew()
        Dim itemval As String, taxval As String, volval As String
        Dim pos As Integer
        resetmessage()
        If lbIndex.SelectedValue Is Nothing Then
            lbIndex.SelectedIndex = 0
        End If
        itemval = pi.GetValue(lbIndex.SelectedIndex)
        ' Example values
        ' "Abies Miller, 1: 37"
        ' "Acrostichum thalictroides L., 1: 11"
        ' "Aeonium Webb & Berth., 1: 429"
        If Not Util.IsEmpty(itemval) Then
            pos = itemval.LastIndexOf(",")
            If pos > -1 Then
                taxval = itemval.Left(pos)
                volval = itemval.Substring(pos + 1)
                ht.CurrentVolume = Util.GetRegexFirstMatch(volval, "^(\d)\s*:")
                cbSelVolum.SelectedItem = ht.CurrentVolume
                ht.CurrentPage = Util.GetRegexFirstMatch(volval, "^\d\s*:\s*(\d+)")
                LoadText(False)

                If Not taxval.IsEmpty Then
                    Me.currentSpecies = ""

                    Dim nfound As Long

                    Me.currentTaxon = Util.GetRegexFirstMatch(taxval, "^(\S+)\s*").Trim
                    Me.currentSpecies = ""

                    nfound = HighlightWords(RichTextBox1, Me.currentTaxon, Color.Yellow)
                    If nfound = 0 Then
                        Me.currentTaxon = sett.GetMetaSyn(Me.currentTaxon)
                        nfound = HighlightWords(RichTextBox1, Me.currentTaxon, Color.Yellow)
                    End If
                    ScrollToLineOfFirstMatch(RichTextBox1, lFirstMatchPosition)

                    Try
                        pi.fillcontrol2(lbSpec, taxval, ht.CurrentVolume)
                    Catch ex As Exception
                        setmessage("Status Message: ", ex.ToString)
                    End Try

                    RichTextBox1.Show()
                    setmessage("Status Message: ", nfound & " entries found")

                End If

                lbSpec.SelectedIndex = -1
                If Me.startFilterFlag And Not Util.IsEmpty(Me.speciesflt) Then
                    Dim i As Integer = 0, flt As String, v2 As String
                    flt = Me.speciesflt.Trim.ToUpper
                    For Each r As DataRowView In lbSpec.Items
                        v2 = r(0)
                        v2 = v2.Trim.ToUpper
                        If v2.StartsWith(flt) Then
                            lbSpec.SelectedIndex = i
                            LbSpecAction()
                            Exit For
                        End If
                        i = i + 1
                    Next
                End If

            End If
        End If
    End Sub

    Public Sub LbIndexAction()

        LbIndexActionNew()
        Exit Sub

        Dim ival As String
        Dim pos1 As Long
        Dim pos2 As Long
        Dim v As String, p As String
        Dim fval As String
        Dim kval As String
        Dim pos3 As Integer
        Dim pos4 As Integer
        resetmessage()
        If lbIndex.SelectedValue Is Nothing Then
            lbIndex.SelectedIndex = 0
        End If
        ival = pi.GetValue(lbIndex.SelectedIndex)
        If Not Util.IsEmpty(ival) Then
            pos1 = lf_Instr(ival, ",")
            If pos1 > 1 Then
                pos2 = lf_Instr(ival, ":", pos1)
                If pos2 > pos1 Then
                    kval = Util.Left(ival, pos1 - 1).Trim
                    v = lf_Substr(ival, pos1 + (pos2 - pos1 - 1), 1)
                    p = lf_Substr(ival, pos2 + 1)
                    ht.CurrentVolume = v
                    cbSelVolum.SelectedItem = ht.CurrentVolume
                    ht.CurrentPage = p
                    LoadText(False)
                    pos3 = lf_Instr(ival, " ")
                    pos4 = lf_Instr(ival, ",")
                    If pos3 = 0 And pos4 > 0 Then
                        pos3 = pos4
                    ElseIf pos4 > 0 And pos4 < pos3 Then
                        pos3 = pos4
                    End If
                    If pos3 > 1 Then
                        fval = Util.Left(ival, pos3 - 1)
                    Else
                        fval = ival
                    End If
                    If fval.Length > 0 Then
                        Dim nfound As Long
                        Me.currentTaxon = fval
                        Me.currentSpecies = ""
                        nfound = HighlightWords(RichTextBox1, fval, Color.Yellow)
                        If nfound = 0 Then
                            fval = sett.GetMetaSyn(fval)
                            nfound = HighlightWords(RichTextBox1, fval, Color.Yellow)
                        End If
                        ScrollToLineOfFirstMatch(RichTextBox1, lFirstMatchPosition)
                        Try
                            pi.fillcontrol2(lbSpec, kval, v)
                        Catch ex As Exception
                            setmessage("Status Message: ", ex.ToString)
                        End Try
                        RichTextBox1.Show()
                        setmessage("Status Message: ", nfound & " entries found")
                    End If
                    lbSpec.SelectedIndex = -1
                    If Me.startFilterFlag And Not Util.IsEmpty(Me.speciesflt) Then
                        Dim i As Integer = 0, flt As String, v2 As String
                        flt = Me.speciesflt.Trim.ToUpper
                        For Each r As DataRowView In lbSpec.Items
                            v2 = r(0)
                            v2 = v2.Trim.ToUpper
                            If v2.StartsWith(flt) Then
                                lbSpec.SelectedIndex = i
                                LbSpecAction()
                                Exit For
                            End If
                            i = i + 1
                        Next
                    End If
                End If
            End If
        End If
    End Sub

    Public Sub LbSpecAction()
        Dim ival As String, kval As String = "", pos1 As Long, pos2 As Long
        Dim p As String = "", x() As String
        resetmessage()
        Me.Cursor = Cursors.WaitCursor
        Try
            ival = pi.GetValue2(lbSpec.SelectedIndex, ht.CurrentVolume)
        Catch ex As Exception
            errmessage(ex.Message)
            ival = ""
        End Try
        Try
            If Not Util.IsEmpty(ival) Then
                pos1 = ival.LastIndexOf(",")
                If pos1 > 1 Then
                    kval = Util.Left(ival, pos1).Trim
                    p = lf_Substr(ival, pos1 + 2)
                    pos2 = lf_Instr(p, "(")
                    If pos2 > 0 Then
                        p = Util.Left(p, pos2 - 1)
                    End If
                    ht.CurrentPage = p.Trim
                End If
            End If
        Catch ex As Exception
            errmessage(ex.Message)
            kval = ""
            p = ""
        End Try
        Try
            If Util.IsEmpty(kval) Then
                x = Util.Matches(ival, "^(.+)\,\s*(\d{1,4})\s*\(\d{1,4}\,*\s*\d*\)\.*$")
                If x IsNot Nothing Then
                    If x.Length > 1 Then
                        p = x(x.Length - 1).Trim
                        kval = x(x.Length - 2).Trim
                        ht.CurrentPage = p.Trim
                    End If
                End If
            End If
        Catch ex As Exception
            errmessage(ex.Message)
            kval = ""
            p = ""
        End Try

        If Not Util.IsEmpty(kval) Then
            LoadText(False)
            Dim nfound As Long
            pos1 = kval.IndexOf(" ")
            If pos1 > 1 Then
                Me.currentSpecies = Util.Left(kval, pos1)
            Else
                Me.currentSpecies = kval
            End If
            nfound = HighlightWords(RichTextBox1, kval, Color.Yellow)
            ScrollToLineOfFirstMatch(RichTextBox1, lFirstMatchPosition)
            RichTextBox1.Show()
            setmessage("Status Message: ", nfound & " entries found")
        Else
            LoadText()
            setmessage("Status Message: ", "not found")
        End If

        Me.Cursor = Cursors.Default
    End Sub

    Private Sub LoadText(Optional ByVal showflag As Boolean = True)

        resetmessage()
        RichTextBox1.Hide()
        Try
            RichTextBox1.LoadFile(ht.CurrentFilePath)
            txtPage.Text = ht.CurrentPage
            btGoto.Enabled = False
            Win32Api.Top(RichTextBox1)
        Catch ex As Exception
        End Try
        If showflag Then
            RichTextBox1.Show()
        End If

    End Sub


    Private Sub FEForma1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Cursor = Cursors.WaitCursor
        lockEvents = True

        If sett Is Nothing Then
            StartMain.Main()
        Else
            LoadCmdLineArgs()
            StartMain.AnaCmd()
        End If

        Me.SetStartFlt(StartMain.CmdSearchGenus, StartMain.CmdSearchSpecies)

        Me.WindowState = FormWindowState.Normal

        Me.MaximizedBounds = System.Windows.Forms.Screen.FromHandle(Me.Handle).WorkingArea
        Me.Location = Me.MaximizedBounds.Location
        Dim w As Integer = Me.MaximizedBounds.Width / 2
        Dim h As Integer = Me.MaximizedBounds.Height / 2
        Me.Size = New Size(w, h)

        resetmessage()
        If sett?.TextStatusOk Then
            cbSelVolum.SelectedItem = "Volume 1"
        End If
        If sett?.DBStatusOk Then
            pi.fillcontrol(lbIndex)
            lbIndex.SelectedIndex = -1
            lbSpec.SelectedIndex = -1
            Me.Cursor = Cursors.Default
        End If
        If Not sett?.SearchStatusOK Then
            Me.btSearch.Enabled = False
        End If
        If Not sett?.FloraExeStatusOK Then
            Me.btGlossary.Enabled = False
        End If

        If startFilterFlag Then
            SetFlt()
            LbIndexAction()
        End If

        If Not sett.SettingStatusOk Then
            FEForma4.Show()
            FEForma4.Activate()
        Else
            Me.Activate()
        End If

        startFilterFlag = False
        lockEvents = False
    End Sub

    Private Sub btBegin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btBegin.Click
        ht.FirstIndex()
        LoadText()
    End Sub

    Private Sub btBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btBack.Click
        ht.PrevIndex()
        LoadText()
    End Sub

    Private Sub btNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btNext.Click
        ht.NextIndex()
        LoadText()
    End Sub

    Private Sub btEnd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btEnd.Click
        ht.LastIndex()
        LoadText()
    End Sub

    Private Sub cbSelVolum_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbSelVolum.SelectedIndexChanged
        Dim cv As String
        If sett?.TextStatusOk Then
            cv = cbSelVolum.SelectedItem.ToString
            resetmessage()
            Select Case cv
                Case "Volume 1"
                    ht.DoVolX("A1")
                Case "Volume 2"
                    ht.DoVolX("A2")
                Case "Volume 3"
                    ht.DoVolX("A3")
                Case "Volume 4"
                    ht.DoVolX("A4")
                Case "Volume 5"
                    ht.DoVolX("A5")
            End Select
            lbSpec.DataSource = Nothing
            Me.Cursor = Cursors.WaitCursor
            ht.CurrentVolume = cv
            LoadText()
            setmessage("Status Message: ", cv)
            Me.Cursor = Cursors.Default
        End If
    End Sub

    Private Sub btQuit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btQuit.Click
        ExitApplication()
    End Sub

    Private Sub btSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btSearch.Click

        resetmessage()
        If Util.IsEmpty(txtFind.Text.Trim) Then
            Exit Sub
        End If

        Dim cmd As String, cmdexe As String, outf As String, line As String
        Dim wf As New LFFile, m As String
        Dim pos1 As Short, pos2 As Short
        Dim foundflag As Boolean = False
        Dim nfound As Long = 0

        Me.Cursor = Cursors.WaitCursor
        wf.openfileforwrite(sett.HomeDir, "ffind.bat")

        cmd = """$$GAISEXED$""  -d -file -i -O -word -w -H ""$$GAISVOL$"" $$S$> ""$$OUTFILE$"""
        cmd = Replace(cmd, "$$GAISEXED$", sett.GaisExe)
        cmd = Replace(cmd, "$$GAISVOL$", sett.GaisVol)
        cmd = Replace(cmd, "$$OUTFILE$", sett.GetOption("OUTFILE"))
        cmd = Replace(cmd, "$$S$", txtFind.Text.Trim)

        wf.putline(cmd)

        cmdexe = Replace("""$EXE$""", "$EXE$", wf.Filename)
        wf.closefile()

        Shell(cmdexe, AppWinStyle.Hide, True, 20000)
        outf = Replace("$$OUTFILE$", "$$OUTFILE$", sett.GetOption("OUTFILE"))
        wf.openfileforread(outf)
        cbSearchresult.Items.Clear()
        cbSearchresult.Text = ""
        While (Not wf.fEOF) And nfound < 101
            line = wf.getline
            line = UCase(line)
            If Not Util.IsEmpty(line) Then
                foundflag = True
                pos1 = lf_Instr(line, "\A", 1)
                pos2 = lf_Instr(line, ".RTF", 1)

                If pos1 > 0 And pos2 > pos1 Then
                    m = lf_GetSubstr(line, pos1 + 1, pos2 + 3)
                Else
                    m = ""
                End If

                Select Case Util.Left(m, 2)
                    Case "A1"
                        m = "Volume 1: Page " & lf_Substr(m, 3)
                    Case "A2"
                        m = "Volume 2: Page " & lf_Substr(m, 3)
                    Case "A3"
                        m = "Volume 3: Page " & lf_Substr(m, 3)
                    Case "A4"
                        m = "Volume 4: Page " & lf_Substr(m, 3)
                    Case "A5"
                        m = "Volume 5: Page " & lf_Substr(m, 3)
                End Select

                m = Replace(m, ".RTF", "")

                If Util.IsEmpty(m) Then
                    cbSearchresult.Items.Add(line)
                Else
                    cbSearchresult.Items.Add(m)
                    nfound = nfound + 1
                End If
            End If
        End While
        If foundflag Then
            cbSearchresult.Text = "Search Results"
        Else
            cbSearchresult.Text = "Not found"
        End If
        wf.closefile()
        If nfound > 100 Then
            setmessage("Status Message: ", "more than 100 pages found")
        ElseIf nfound = 1 Then
            setmessage("Status Message: ", 1 & " page found")
        ElseIf nfound = 0 Then
            setmessage("Status Message: ", "nothing found")
        Else
            setmessage("Status Message: ", nfound & " pages found")
        End If
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub cbSearchresult_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbSearchresult.SelectedIndexChanged
        Dim cv As String, v As String, page As Long, pos1 As Short, nfound As Long
        resetmessage()
        cv = cbSearchresult.SelectedItem.ToString
        Me.Cursor = Cursors.WaitCursor
        lbSpec.DataSource = Nothing
        v = Util.Left(cv, 8)
        Select Case v
            Case "Volume 1"
                ht.DoVolX("A1")
                ' cbSelVolum.
            Case "Volume 2"
                ht.DoVolX("A2")
            Case "Volume 3"
                ht.DoVolX("A3")
            Case "Volume 4"
                ht.DoVolX("A4")
            Case "Volume 5"
                ht.DoVolX("A5")
        End Select
        ht.CurrentVolume = v
        cbSelVolum.SelectedItem = v
        pos1 = lf_Instr(cv, "Page")
        page = lf_Substr(cv, pos1 + 5)
        If pos1 > 1 Then
            ht.CurrentIndex = page
        End If

        LoadText(False)
        nfound = HighlightWords(RichTextBox1, txtFind.Text, Color.Yellow)
        ScrollToLineOfFirstMatch(RichTextBox1, lFirstMatchPosition)
        RichTextBox1.Show()
        setmessage("Status Message: ", nfound & " entries found")
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub txtPage_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPage.TextChanged
        btGoto.Enabled = True
    End Sub

    Private Sub btGoto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btGoto.Click
        ht.CurrentPage = txtPage.Text
        LoadText()
        setmessage("Status Message: ", "Page: " & txtPage.Text)
        txtPage.Text = ht.CurrentPage
    End Sub

    Private Sub lbIndex_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles lbIndex.KeyPress
        LbIndexAction()
    End Sub

    Private Sub lbIndex_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbIndex.Click
        LbIndexAction()
    End Sub

    Private Sub lbSpec_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbSpec.Click
        LbSpecAction()
    End Sub

    Private Sub lbspec_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles lbSpec.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Return) Then
            LbSpecAction()
        End If
    End Sub

    Private Function LookupGlossary() As Boolean
        Dim t As String, sel As GlossaryElement, t1(1) As String
        t = Trim(RichTextBox1.SelectedText)
        If Util.IsEmpty(t) Then
            t = Trim(txtInfo.SelectedText)
            If Util.IsEmpty(t) Then
                t = Trim(txtFind.Text)
            End If
        End If
        sel = gl.Search(t)
        If sel Is Nothing Then
            txtInfo.Text = "Nothing found"
            setmessage("Glossary: ", "Nothing found for /" & t & "/")
            LookupGlossary = False
        Else
            t1(0) = sel.Term & ": "
            t1(1) = sel.Description
            txtInfo.Lines = t1
            setmessage("Glossary: ", "-> /" & t & "/")
            LookupGlossary = True
        End If
    End Function

    Private Sub btGlossary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btGlossary.Click
        If Not LookupGlossary() Then
            Dim t As String
            t = Trim(RichTextBox1.SelectedText)
            If Util.IsEmpty(t) Then
                t = Trim(txtInfo.SelectedText)
                If Util.IsEmpty(t) Then
                    t = Trim(txtFind.Text)
                End If
            End If
            If t.Length > 0 Then
                gl.setFilter(Strings.Left(t, 2))
                gl.setfilterOn()
                FEForma3.Activate()
                FEForma3.Show()
                setmessage("Glossary: ", "browse for " & Strings.Left(t, 2))
            End If
        End If
    End Sub

    Private Sub btFindInPage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btFindInPage.Click
        Dim t As String
        t = txtFind.Text.Trim()
        resetmessage()
        If t.Length > 0 Then
            LoadText(False)
            Dim nfound As Long
            nfound = HighlightWords(RichTextBox1, txtFind.Text, Color.Yellow)
            ScrollToLineOfFirstMatch(RichTextBox1, lFirstMatchPosition)
            setmessage("Status Message: ", nfound & " entries found")
            RichTextBox1.Show()
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        ExitApplication()
    End Sub

    Private Sub CopyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripMenuItem.Click
        If RichTextBox1.Focused Then
            RichTextBox1.Copy()
        ElseIf txtInfo.Focused Then
            txtInfo.Copy()
        ElseIf txtFind.Focused Then
            txtFind.Copy()
        ElseIf txtFilter.Focused Then
            txtFilter.Copy()
        End If
    End Sub

    Private Sub PasteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PasteToolStripMenuItem.Click
        If txtFind.Focused Then
            txtFind.Paste()
        ElseIf txtInfo.Focused Then
            txtInfo.Paste()
        ElseIf txtFilter.Focused Then
            txtFilter.Paste()
        End If
    End Sub

    Private Sub SelectAllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectAllToolStripMenuItem.Click
        If RichTextBox1.Focused Then
            RichTextBox1.SelectAll()
        ElseIf txtInfo.Focused Then
            txtInfo.SelectAll()
        ElseIf txtFind.Focused Then
            txtFind.SelectAll()
        ElseIf txtFilter.Focused Then
            txtFilter.SelectAll()
        End If
    End Sub

    Private Sub FindGlobalToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        btSearch_Click(sender, e)
    End Sub

    Private Sub FindLocalToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        btFindInPage_Click(sender, e)
    End Sub

    Private Sub IntroductionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IntroductionToolStripMenuItem.Click
        If lf_fileExists2(sett.HomeDir, "FloraEuropaea.chm") = 1 Then
            Help.ShowHelp(Me, "FloraEuropaea.chm")
        ElseIf lf_fileExists2(sett.HomeDir, "FLORAHELP.HLP") = 1 Then
            Help.ShowHelp(Me, "FLORAHELP.HLP")
        Else
            MessageBox.Show("Data not available")
        End If
    End Sub

    Private Sub GlossaryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GlossaryToolStripMenuItem.Click
        FEForma3.Activate()
        FEForma3.ShowDialog()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        AboutBoxLF.ShowDialog()
        AboutBoxLF.Activate()
    End Sub

    Private Sub btKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btKey.Click
        Dim t As String, k As Key
        t = Trim(RichTextBox1.SelectedText)
        If Util.IsEmpty(t) Then
            t = currentTaxon
            If Util.IsEmpty(t) Then
                t = Trim(txtFind.Text)
            End If
        End If
        k = kc.GetKey(t)
        If k Is Nothing Then
            t = currentTaxon
            If Util.IsEmpty(t) Then
                t = Trim(txtFind.Text)
            End If
            k = kc.GetKey(t)
        End If
        If k IsNot Nothing Then
            FEForma2.CurrentKey = k
            FEForma2.Show()
            FEForma2.Activate()
            setmessage("Key ", k.KeyName)
        Else
            setmessage("Key ", "/" & t & "/" & " not found")
        End If
    End Sub

    ' Printing
    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint
        checkPrint = 0
    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        ' Print the content of the RichTextBox. Store the last character printed.
        checkPrint = Win32Api.Print(RichTextBox1, checkPrint, RichTextBox1.TextLength, e)
        ' Look for more pages
        If checkPrint < RichTextBox1.TextLength Then
            e.HasMorePages = True
        Else
            e.HasMorePages = False
        End If
    End Sub

    Private Sub PrintPreviewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintPreviewToolStripMenuItem.Click
        PrintPreviewDialog1.ShowDialog()
    End Sub

    Private Sub PrintToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintToolStripMenuItem.Click
        If PrintDialog1.ShowDialog() = DialogResult.OK Then
            PrintDocument1.Print()
        End If
    End Sub

    Private Sub PageSetupToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PageSetupToolStripMenuItem.Click
        PageSetupDialog1.ShowDialog()
    End Sub

    Private Sub KeyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KeyToolStripMenuItem.Click
        Dim t As String = "TAXA", k As Key
        k = kc.GetKey(t)
        If k IsNot Nothing Then
            FEForma2.CurrentKey = k
            FEForma2.Show()
            FEForma2.Activate()
            setmessage("Key ", k.KeyName)
        Else
            setmessage("Key ", "/" & t & "/" & " not found")
        End If
    End Sub

    Private Sub txtFind_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFind.TextChanged
        If txtFind.Text.ToString.Length > 3 Then
            btFindInPage.Enabled = True
            btSearch.Enabled = True
            resetmessage()
        Else
            setmessage("Search text should be more than 3 characters long")
            btFindInPage.Enabled = False
            btSearch.Enabled = False
        End If
    End Sub

    Private Sub ckFilter_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ckFilter.CheckedChanged
        If ckFilter.Checked Then
            SetFlt()
        Else
            ResetFlt()
        End If
    End Sub

    Private Sub KeyToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            FEForma2.Activate()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub GlossaryToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            FEForma3.Activate()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btSetFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btSetFilter.Click
        If txtFilter.Text.ToString.Length > 0 Then
            If Not ckFilter.Checked Then
                ckFilter.Checked = True
            End If
            SetFlt()
        Else
            ResetFlt()
        End If
    End Sub

    Private Sub SettingsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SettingsToolStripMenuItem.Click
        Try
            FEForma4.ShowDialog()
        Catch ex As Exception
        End Try
        FEForma4.Activate()
    End Sub

    Private Sub Richtextbox1_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles RichTextBox1.MouseDoubleClick
        Dim t As String, k As Key
        t = Trim(RichTextBox1.SelectedText).Trim
        k = kc.GetKey(t)
        If k IsNot Nothing Then
            FEForma2.CurrentKey = k
            FEForma2.Show()
            FEForma2.Activate()
            setmessage("Key ", k.KeyName)
        Else
            LookupGlossary()
        End If
    End Sub

    Private Sub HelpToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HelpToolStripMenuItem1.Click
        Help.ShowHelp(Me, "GUIFE.CHM")
    End Sub

    Private Sub HomePageStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HomePageStripMenuItem.Click
        Dim webAddress As String = "https://www.foerderer.ch/downloads.html"
        OpenUrl(webAddress)
    End Sub

    Private Sub DeleteCacheToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteCacheToolStripMenuItem.Click
        sett.DeleteCache = True
        sett.SaveSettings()
    End Sub

    Private Sub lbIndex_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbIndex.SelectedIndexChanged
        If Not Me.lockEvents Then
            Me.LbIndexAction()
        End If
    End Sub

    Private Sub lbSpec_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSpec.SelectedIndexChanged
        If Not Me.lockEvents Then
            Me.LbSpecAction()
        End If
    End Sub

    Private Sub UserDataToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UserDataToolStripMenuItem.Click

        Dim uc As UserCmd
        If FEUser.ShowDialog() = Windows.Forms.DialogResult.OK Then
            uc = FEUser.SelectedUserCmdProperty
            uc.ParProperty = GetClip()
            uc.ExecUserCmd()
        End If

    End Sub

    Private Sub GoogleToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GoogleToolStripMenuItem.Click
        Dim uc As New UserCmd("Google", "GOOGLE", "LINK", "https://www.google.ch/search?q=xxPx")
        Dim t As String
        t = Trim(RichTextBox1.SelectedText)
        If Util.IsEmpty(t) Then
            t = Trim(txtInfo.SelectedText)
            If Util.IsEmpty(t) Then
                t = Trim(txtFind.Text)
                If Util.IsEmpty(t) Then
                    Try
                        t = Me.currentTaxon.Trim
                        If Not Util.IsEmpty(Me.currentSpecies) Then
                            t = t & " " & (Me.currentSpecies.Trim)
                        End If
                    Catch ex As Exception
                        MsgBox("No taxon selected!")
                    End Try
                End If
            End If
        End If
        If Not Util.IsEmpty(t) Then
            uc.ParProperty = t
        End If
        uc.ExecUserCmd()
    End Sub

    Private Sub MaximizeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MaximizeToolStripMenuItem.Click
        Me.MaximizedBounds = System.Windows.Forms.Screen.FromHandle(Me.Handle).WorkingArea
        Me.Location = Me.MaximizedBounds.Location
        Me.Size = Me.MaximizedBounds.Size
    End Sub

    Private Sub MinimizeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MinimizeToolStripMenuItem.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub SmallToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SmallToolStripMenuItem.Click
        ' Set form to half the size of the screen
        Me.WindowState = FormWindowState.Normal

        Me.MaximizedBounds = System.Windows.Forms.Screen.FromHandle(Me.Handle).WorkingArea
        Me.Location = Me.MaximizedBounds.Location
        Dim w As Integer = Me.MaximizedBounds.Width / 2
        Dim h As Integer = Me.MaximizedBounds.Height / 2
        Me.Size = New Size(w, h)
    End Sub
End Class
