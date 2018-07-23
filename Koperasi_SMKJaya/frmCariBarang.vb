Public Class frmCariBarang

    Dim sql As String

    Private Sub resetForm()
        TextBox1.Clear()
        TextBox1.Focus()
    End Sub

    Private Sub refreshGrid()
        sql = "select * from q_jenisbarang where kd_jenis_barang like '%" & TextBox1.Text & "%' or jenis_barang like '%" & TextBox1.Text & "%' or kd_barang like '%" & TextBox1.Text & "%' or nama_barang like '%" & TextBox1.Text & "%'"

        fetchData(sql, "q_jenisbarang", DataGridView1, Label5)
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        refreshGrid()
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Try
            If Me.Owner Is frmPenjualanAnggota Then
                frmPenjualanAnggota.TextBox2.Text = DataGridView1.Item(0, e.RowIndex).Value
            Else
                frmPenjualanUmum.TextBox2.Text = DataGridView1.Item(0, e.RowIndex).Value
            End If
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

    Private Sub frmCariBarang_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        resetForm()
        refreshGrid()
    End Sub
End Class