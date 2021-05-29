Imports System.Runtime.Serialization.Formatters.Binary
Imports System.IO
Imports System.IO.Compression
Imports System.Text.Json
Imports LfUtilities
Imports System.Xml.Serialization

' Source: http://www.vbarchiv.net/tipps/details.php?id=1749
Public Class Serializer
    Public Shared Sub JsonSerialize(Of T)(ByVal compression As Boolean,
      ByVal path As String, ByVal instance As T)
        Dim jsonString As String
        Try

            Dim options = New JsonSerializerOptions()
            options.MaxDepth = 0
            options.IgnoreReadOnlyProperties = True
            options.IgnoreNullValues = True

            jsonString = JsonSerializer.Serialize(instance, options)
            If compression Then
                jsonString = jsonString.Compress()
            End If
            File.WriteAllText(path, jsonString)
        Catch ex As Exception
            MessageBox.Show(ex.Message, Application.ProductName,
              MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Shared Sub JsonSerialize(Of T)(ByVal path As String, ByVal instance As T)
        JsonSerialize(False, path, instance)
    End Sub

    Public Shared Sub Serialize(Of T)(ByVal compression As Boolean,
      ByVal path As String, ByVal instance As T)

        Try
            Dim fs As Stream = New FileStream(path, FileMode.OpenOrCreate)
            Dim bf As New BinaryFormatter
            If compression Then fs = New GZipStream(fs, CompressionMode.Compress)

            bf.Serialize(fs, instance)
            fs.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, Application.ProductName,
              MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Shared Sub Serialize(Of T)(ByVal path As String, ByVal instance As T)
        Serialize(False, path, instance)
    End Sub

    Public Shared Sub XMLSerialize(Of T)(ByVal compression As Boolean,
      ByVal path As String, ByVal instance As T)

        Try
            Dim fs As Stream = New FileStream(path, FileMode.OpenOrCreate)
            Dim xf As New XmlSerializer(GetType(T))
            If compression Then fs = New GZipStream(fs, CompressionMode.Compress)
            xf.Serialize(fs, instance)
            fs.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, Application.ProductName,
              MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Shared Sub XMLSerialize(Of T)(ByVal path As String, ByVal instance As T)
        XMLSerialize(False, path, instance)
    End Sub

    Public Shared Function DeSerialize(Of T)(ByVal compression As Boolean,
      ByVal path As String, ByVal defaultInstance As T) As T

        Try
            If Not File.Exists(path) Then
                Return defaultInstance
            End If
            Dim fs As Stream = New FileStream(path, FileMode.OpenOrCreate)
            Dim bf As New BinaryFormatter
            If compression Then fs = New GZipStream(fs, CompressionMode.Decompress)

            DeSerialize = CType(bf.Deserialize(fs), T)
            fs.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, Application.ProductName,
              MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

    Public Shared Function DeSerialize(Of T)(ByVal path As String,
      ByVal defaultInstance As T) As T

        Return DeSerialize(Of T)(False, path, defaultInstance)
    End Function

    Public Shared Function DeSerialize(Of T As New)(ByVal path As String) As T
        Return DeSerialize(Of T)(path, New T)
    End Function

    Public Shared Function DeSerialize(Of T As New)(
      ByVal compression As Boolean, ByVal path As String) As T

        Return DeSerialize(Of T)(compression, path, New T)
    End Function

    Public Shared Function DeXMLSerialize(Of T)(ByVal compression As Boolean,
      ByVal path As String, ByVal defaultInstance As T) As T

        Try
            If Not File.Exists(path) Then
                Return defaultInstance
            End If
            Dim fs As Stream = New FileStream(path, FileMode.OpenOrCreate)
            Dim xf As New XmlSerializer(GetType(T))
            If compression Then fs = New GZipStream(fs, CompressionMode.Decompress)

            DeXMLSerialize = CType(xf.Deserialize(fs), T)
            fs.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, Application.ProductName,
              MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

    Public Shared Function DeXMLSerialize(Of T)(ByVal path As String,
      ByVal defaultInstance As T) As T

        Return DeXMLSerialize(Of T)(False, path, defaultInstance)
    End Function

    Public Shared Function DeXMLSerialize(Of T As New)(ByVal path As String) As T
        Return DeXMLSerialize(Of T)(path, New T)
    End Function

    Public Shared Function DeXMLSerialize(Of T As New)(
      ByVal compression As Boolean, ByVal path As String) As T

        Return DeXMLSerialize(Of T)(compression, path, New T)
    End Function

    'Each parameter in constructor 'Void .ctor(System.String, System.String)' on type 'GUIFE.GlossaryElement' 
    ' must bind To an Object Property Or field On deserialization. Each parameter name must match With a Property Or 
    ' field On the Object. The match can be Case-insensitive.
    Public Shared Function DeJsonSerialize(Of T)(ByVal compression As Boolean,
  ByVal path As String, ByVal defaultInstance As T) As T

        Try
            If Not File.Exists(path) Then
                Return defaultInstance
            End If
            Dim jsonString As String = File.ReadAllText(path)
            If compression Then
                jsonString = jsonString.Decompress()
            End If
            Dim r As T
            r = JsonSerializer.Deserialize(jsonString, GetType(T))

            Return r
        Catch ex As Exception
            MessageBox.Show(ex.Message, Application.ProductName,
              MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function
    Public Shared Function DeJsonSerialize(Of T)(ByVal path As String,
  ByVal defaultInstance As T) As T

        Return DeJsonSerialize(Of T)(False, path, defaultInstance)
    End Function

    Public Shared Function DeJsonSerialize(Of T As New)(ByVal path As String) As T
        Return DeJsonSerialize(Of T)(path, New T)
    End Function

    Public Shared Function DeJsonSerialize(Of T As New)(
      ByVal compression As Boolean, ByVal path As String) As T

        Return DeJsonSerialize(Of T)(compression, path, New T)
    End Function

End Class

