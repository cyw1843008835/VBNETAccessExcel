Imports Microsoft.Office.Interop
Imports Microsoft.Office.Interop.Excel

REM 需要下载插件Microsoft.Office.Interop.Excel
Public Class Form1
    Dim objexcelfile As Excel.Application
    Dim objworkbook As Excel.Workbook
    Dim objimportsheet As Excel.Worksheet
    Dim intlastrownum, intcount As Integer



    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim modelCollection As New Collection
        Dim evidenceCollection As New Collection
        Dim Mcount， Ecount As Integer
        modelCollection = ReadExcel(file_TextBox.Text, ComboBox1.SelectedItem.ToString)
        evidenceCollection = ReadExcel(file_TextBox.Text, ComboBox2.SelectedItem.ToString)

        For Ecount = 1 To evidenceCollection.Count

            For Mcount = 1 To modelCollection.Count
                If evidenceCollection(Ecount) = modelCollection(Mcount) Then
                    modelCollection.Remove(Mcount)
                    Exit For
                End If
            Next
        Next

        For Mcount = 1 To modelCollection.Count
            ListBox1.Items.Add(modelCollection(Mcount))
        Next
    End Sub

    Private Sub file_TextBox_Click(sender As Object, e As EventArgs) Handles file_TextBox.Click
        OpenFileDialog1.ShowDialog()
        file_TextBox.Text = OpenFileDialog1.FileName
    End Sub
    Private Sub folder_TextBox_Click(sender As Object, e As EventArgs) Handles folder_TextBox.Click
        FolderBrowserDialog1.ShowDialog()
        folder_TextBox.Text = FolderBrowserDialog1.SelectedPath
    End Sub

    Function ReadExcel(filename As String, SheetName As String) As Collection
        Dim resultCollection As New Collection

        objexcelfile = New Excel.Application
        objexcelfile.DisplayAlerts = False
        objworkbook = objexcelfile.Workbooks.Open(filename)
        objimportsheet = objworkbook.Sheets(SheetName)

        intlastrownum = objimportsheet.UsedRange.Rows.Count

        For intcount = 1 To intlastrownum
            If objimportsheet.Cells(intcount, 1).value <> "" Then
                resultCollection.Add(objimportsheet.Cells(intcount, 1).value)
            End If
        Next
        CloseExcel(objexcelfile, objworkbook, objimportsheet)
        Return resultCollection
    End Function

    Private Sub file_TextBox_TextChanged(sender As Object, e As EventArgs) Handles file_TextBox.TextChanged
        Dim ShCount As Integer
        objexcelfile = New Excel.Application
        objexcelfile.DisplayAlerts = False
        objworkbook = objexcelfile.Workbooks.Open(file_TextBox.Text)
        For ShCount = 1 To objworkbook.Worksheets.Count
            ComboBox1.Items.Add(objworkbook.Worksheets(ShCount).name)
            ComboBox2.Items.Add(objworkbook.Worksheets(ShCount).name)
        Next
        CloseExcel(objexcelfile, objworkbook, objimportsheet)
    End Sub

    Private Sub CloseExcel(objexcelfile As Excel.Application, objworkbook As Excel.Workbook, objimportsheet As Excel.Worksheet)
        Try
            objworkbook = Nothing
            objimportsheet = Nothing
            objexcelfile = Nothing
            objexcelfile.Quit()


        Catch ex As Exception
            Console.WriteLine(ex)

        End Try

        GC.Collect()
        GC.WaitForPendingFinalizers()
    End Sub
End Class
