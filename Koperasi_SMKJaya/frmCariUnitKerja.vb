Public Class frmCariUnitKerja

    Dim sql As String

    Private Sub resetForm()
        TextBox1.Clear()
        TextBox1.Focus()
    End Sub

    Private Sub refreshGrid()
        sql = "select * from tblunitkerja where kd_unit_kerja like '%" & TextBox1.Text & "%' or unit_kerja like '%" & TextBox1.Text & "%'"

        fetchData(sql, "tblunitkerja", DataGridView1, Label5)
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        refreshGrid()
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Try
            frmAnggota.ComboBox1.SelectedValue = DataGridView1.Item(0, e.RowIndex).Value
            Close()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        WindowState = FormWindowState.Minimized
    End Sub

    Private Sub frmCariUnitKerja_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        resetForm()
        refreshGrid()
    End Sub
End Class