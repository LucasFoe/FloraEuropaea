Imports System.Data.OleDb
Imports System.Threading
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.IO
Imports LfUtilities

Public Class PlantIndex
    Implements IDisposable

    Private clPNAME As New Dictionary(Of String, DataRow)

    Private schema As DataTable
    Private dvSchema As DataView

    Private dvPlantName As DataView
    Private dvVol1 As DataView
    Private dvVol2 As DataView
    Private dvVol3 As DataView
    Private dvVol4 As DataView
    Private dvVol5 As DataView

    Private dtPlantName As DataTable
    Private dtVol1 As DataTable
    Private dtVol2 As DataTable
    Private dtVol3 As DataTable
    Private dtVol4 As DataTable
    Private dtVol5 As DataTable

    Private da As OleDbDataAdapter
    Private dsIndex As DataSet

    Private Sub addcollPname(ByVal k As String, ByRef r As DataRow)
        If ExistsInclPNAME(k) Then
            clPNAME.Remove(k)
        End If
        Try
            clPNAME.Add(k, r)
        Catch ex As Exception
        End Try
    End Sub

    Public Function GetclPNAME(ByVal k As String) As DataRow
        If ExistsInclPNAME(k) Then
            Try
                Return clPNAME.Item(k)
            Catch ex As Exception
                Return Nothing
            End Try
        Else
            Return Nothing
        End If
    End Function

    Public Function ExistsInclPNAME(ByVal k As String) As Boolean
        Try
            Return clPNAME.ContainsKey(k)
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub fillPNAME(ByRef dt As DataTable, Optional ByVal fillcollflag As Boolean = False)
        Dim MyDataRow As DataRow, w As String, pos As Long, isSpecFlag As Boolean
        Dim GENUS As String = ""
        dt.Columns.Add("PNAME", System.Type.GetType("System.String"))
        If Not fillcollflag Then
            dt.Columns.Add("ISSPECFLAG", System.Type.GetType("System.Boolean"))
            dt.Columns.Add("GENUS", System.Type.GetType("System.String"))
        End If

        For Each MyDataRow In dt.Rows
            w = MyDataRow("PlantName")
            pos = lf_Instr(w, ",")
            If pos > 1 Then
                w = Util.Left(w, pos - 1)
            End If
            MyDataRow("PNAME") = w
            If fillcollflag Then
                addcollPname(w, MyDataRow)
            Else
                If lf_FirstLowerCase(w) Then
                    isSpecFlag = True
                Else
                    isSpecFlag = False
                End If
                MyDataRow("ISSPECFLAG") = isSpecFlag
                If Not isSpecFlag Then
                    GENUS = w
                End If
                MyDataRow("GENUS") = GENUS
            End If
            MyDataRow.AcceptChanges()
        Next

    End Sub

    Sub filv1()
        ' Index fuer Vol 1
        da = New OleDbDataAdapter("SELECT PLANTNAME, RECORDNUMBER FROM INDEXV1 ORDER BY RECORDNUMBER", connStrIndex)
        dtVol1 = New DataTable("Vol1")
        da.Fill(dtVol1)
        fillPNAME(dtVol1)
    End Sub

    Sub filv2()
        ' Index fuer Vol 2
        da = New OleDbDataAdapter("SELECT PLANTNAME, RECORDNUMBER FROM INDEXV2 ORDER BY RECORDNUMBER", connStrIndex)
        dtVol2 = New DataTable("Vol2")
        da.Fill(dtVol2)
        fillPNAME(dtVol2)
    End Sub

    Sub filv3()
        ' Index fuer Vol 3
        da = New OleDbDataAdapter("SELECT PLANTNAME, RECORDNUMBER FROM INDEXV3 ORDER BY RECORDNUMBER", connStrIndex)
        dtVol3 = New DataTable("Vol3")
        da.Fill(dtVol3)
        fillPNAME(dtVol3)
    End Sub

    Sub filv4()
        ' Index fuer Vol 4
        da = New OleDbDataAdapter("SELECT PLANTNAME, RECORDNUMBER FROM INDEXV4 ORDER BY RECORDNUMBER", connStrIndex)
        dtVol4 = New DataTable("Vol4")
        da.Fill(dtVol4)
        fillPNAME(dtVol4)
    End Sub

    Sub filv5()
        ' Index fuer Vol 5
        da = New OleDbDataAdapter("SELECT PLANTNAME, RECORDNUMBER FROM INDEXV5 ORDER BY RECORDNUMBER", connStrIndex)
        dtVol5 = New DataTable("Vol5")
        da.Fill(dtVol5)
        fillPNAME(dtVol5)
    End Sub

    Public Sub New(ByRef con As OleDbConnection)

        Dim dtf As String, readDataFlag As Boolean
        dtf = lf_filename(sett.HomeDir, "2.dat")

        If sett.DeleteCache Then
            readDataFlag = True
        Else
            Try
                If File.Exists(dtf) Then
                    dsIndex = Serializer.DeXMLSerialize(Of DataSet)(True, dtf)

                    dtPlantName = dsIndex.Tables("Plantname")
                    dvPlantName = dtPlantName.DefaultView

                    dtVol1 = dsIndex.Tables("Vol1")
                    dtVol2 = dsIndex.Tables("Vol2")
                    dtVol3 = dsIndex.Tables("Vol3")
                    dtVol4 = dsIndex.Tables("Vol4")
                    dtVol5 = dsIndex.Tables("Vol5")

                    dvVol1 = dtVol1.DefaultView
                    dvVol2 = dtVol2.DefaultView
                    dvVol3 = dtVol3.DefaultView
                    dvVol4 = dtVol4.DefaultView
                    dvVol5 = dtVol5.DefaultView

                    dvPlantName.Sort = "PNAME"
                    dvVol1.Sort = "GENUS"
                    dvVol2.Sort = "GENUS"
                    dvVol3.Sort = "GENUS"
                    dvVol4.Sort = "GENUS"
                    dvVol5.Sort = "GENUS"

                    ' Schema Informationen lesen und mit default DataView versehen
                    schema = dsIndex.Tables("Columns")
                    dvSchema = schema.DefaultView
                    readDataFlag = False
                Else
                    readDataFlag = True
                End If
            Catch ex As Exception
                readDataFlag = True
            End Try
        End If

        If readDataFlag Then

            ' Index fuer Familiem und Gattungen
            da = New OleDbDataAdapter("SELECT PLANTNAME FROM plantind WHERE PLANTNAME LIKE '%,%:%' ORDER BY RECORD", connIndex)
            dtPlantName = New DataTable("Plantname")
            da.Fill(dtPlantName)
            fillPNAME(dtPlantName, True)

            filv1()
            filv2()
            filv3()
            filv4()
            filv5()

            ' DataView erzeugen und Standardansicht zuweisen 
            dvPlantName = dtPlantName.DefaultView
            dvVol1 = dtVol1.DefaultView
            dvVol2 = dtVol2.DefaultView
            dvVol3 = dtVol3.DefaultView
            dvVol4 = dtVol4.DefaultView
            dvVol5 = dtVol5.DefaultView

            dvPlantName.Sort = "PNAME"
            dvVol1.Sort = "GENUS"
            dvVol2.Sort = "GENUS"
            dvVol3.Sort = "GENUS"
            dvVol4.Sort = "GENUS"
            dvVol5.Sort = "GENUS"

            ' Schema Informationen lesen und mit default DataView versehen
            con.Open()
            schema = con.GetSchema("COLUMNS")
            dvSchema = schema.DefaultView
            con.Close()
            dsIndex = New DataSet("Index")
            dsIndex.Tables.Add(schema)
            dsIndex.Tables.Add(dtPlantName)
            dsIndex.Tables.Add(dtVol1)
            dsIndex.Tables.Add(dtVol2)
            dsIndex.Tables.Add(dtVol3)
            dsIndex.Tables.Add(dtVol4)
            dsIndex.Tables.Add(dtVol5)

            ' Dim bf = New BinaryFormatter()
            ' Dim fs = New FileStream(dtf, FileMode.OpenOrCreate)
            ' dsIndex.RemotingFormat = SerializationFormat.Binary
            ' bf.Serialize(fs, dsIndex)
            Serializer.XMLSerialize(True, dtf, dsIndex)
            sett.DeleteCache = False
            sett.SaveSettings()

        End If

    End Sub

    Public Sub DeleteCache()
        Dim dtf As String
        dtf = lf_filename(sett.HomeDir, "planindex.dat")
        Try
            Dim fi As New FileInfo(dtf)
            fi.Delete()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub


    Public Sub fillcontrol(ByRef lc As ListControl, Optional ByVal flt As String = "")
        ' Filterkriterium festlegen
        If Not Util.IsEmpty(flt) Then
            dvPlantName.RowFilter = flt
        Else
            dvPlantName.RowFilter = Nothing
        End If
        ' Schlüsselspalte
        lc.ValueMember = "PLANTNAME"
        ' Anzeigespalte
        lc.DisplayMember = "PLANTNAME"
        ' ListControl anbinden
        lc.DataSource = dvPlantName
    End Sub

    Public Sub setfilter(Optional ByVal flt As String = "")
        If Not Util.IsEmpty(flt) Then
            dvPlantName.RowFilter = flt
        Else
            dvPlantName.RowFilter = Nothing
        End If
    End Sub

    Public Sub fillcontrol2(ByRef lc As ListControl, ByVal pName As String, ByVal vol As String)
        ' Filterkriterium festlegen

        Dim rf As String

        rf = "GENUS = '$$PN$' AND ISSPECFLAG "
        rf = Replace(rf, "$$PN$", pName)

        Select Case vol
            Case "Volume 1", "1"
                dvVol1.RowFilter = rf
                lc.DataSource = dvVol1
            Case "Volume 2", "2"
                dvVol2.RowFilter = rf
                lc.DataSource = dvVol2
            Case "Volume 3", "3"
                dvVol3.RowFilter = rf
                lc.DataSource = dvVol3
            Case "Volume 4", "4"
                dvVol4.RowFilter = rf
                lc.DataSource = dvVol4
            Case "Volume 5", "5"
                dvVol5.RowFilter = rf
                lc.DataSource = dvVol5
        End Select

        lc.DisplayMember = "PLANTNAME"
        lc.ValueMember = "PLANTNAME"

    End Sub

    Public Function GetValue(ByVal idx As Long) As String
        Dim v As String
        If idx >= 0 Then
            v = dvPlantName(idx).Row(0).ToString
        Else
            v = ""
        End If
        Return v
    End Function

    Public Function GetValue2(ByVal idx As Long, ByVal vol As String) As String
        Dim v As String
        v = ""
        Select Case vol
            Case "Volume 1", "1"
                v = dvVol1(idx).Row(0).ToString
            Case "Volume 2", "2"
                v = dvVol2(idx).Row(0).ToString
            Case "Volume 3", "3"
                v = dvVol3(idx).Row(0).ToString
            Case "Volume 4", "4"
                v = dvVol4(idx).Row(0).ToString
            Case "Volume 5", "5"
                v = dvVol5(idx).Row(0).ToString
        End Select
        Return v
    End Function

    Public Sub DumpDataToXML(ByRef dt As DataTable, Optional ByVal filename As String = "")
        If Util.IsEmpty(filename) Then
            filename = dt.TableName.ToString.ToUpper & ".XML"
        End If
        dt.WriteXml(filename)
    End Sub

    Public Sub DumpDataToXML(ByRef dv As DataView, Optional ByVal filename As String = "")
        If Util.IsEmpty(filename) Then
            filename = "dv" & dv.Table.TableName.ToString.ToUpper & ".XML"
        End If
        dv.ToTable.WriteXml(filename)
    End Sub

    Public Sub DumpAllToXML()
        DumpDataToXML(schema)
        DumpDataToXML(dvSchema)
        DumpDataToXML(dtPlantName)
        DumpDataToXML(dtVol1)
        DumpDataToXML(dtVol2)
        DumpDataToXML(dtVol3)
        DumpDataToXML(dtVol4)
        DumpDataToXML(dtVol5)
        DumpDataToXML(dvPlantName)
        DumpDataToXML(dvVol1)
        DumpDataToXML(dvVol2)
        DumpDataToXML(dvVol3)
        DumpDataToXML(dvVol4)
        DumpDataToXML(dvVol5)
    End Sub


#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
                DisposeIfNotNull(schema)
                DisposeIfNotNull(dvSchema)
                DisposeIfNotNull(dvPlantName)
                DisposeIfNotNull(dvVol1)
                DisposeIfNotNull(dvVol2)
                DisposeIfNotNull(dvVol3)
                DisposeIfNotNull(dvVol4)
                DisposeIfNotNull(dvVol5)
                DisposeIfNotNull(dtVol1)
                DisposeIfNotNull(dtVol2)
                DisposeIfNotNull(dtVol3)
                DisposeIfNotNull(dtVol4)
                DisposeIfNotNull(dtVol5)
                DisposeIfNotNull(da)
                DisposeIfNotNull(dsIndex)
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
            clPNAME.Clear()
        End If
        Me.disposedValue = True
    End Sub

    Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

End Class
