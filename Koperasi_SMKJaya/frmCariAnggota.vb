Public Class frmCariAnggota

    Dim sql As String

    Private Sub resetForm()
        TextBox1.Clear()
        TextBox1.Focus()
    End Sub

    Private Sub refreshGrid()
        sql = "select * from q_anggota where kd_anggota like '%" & TextBox1.Text & "%' or nama_anggota like '%" & TextBox1.Text & "%' or unit_kerja like '%" & TextBox1.Text & "%' or alamat like '%" & TextBox1.Text & "%'"

        fetchData(sql, "q_anggota", DataGridView1, Label5)
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        refreshGrid()
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Try
            If Me.Owner Is frmGajian Then
                frmGajian.TextBox3.Text = DataGridView1.Item(3, e.RowIndex).Value
                frmGajian.txtIDAnggota.Text = DataGridView1.Item(0, e.RowIndex).Value
            ElseIf Me.Owner Is frmPenjualanAnggota Then
                frmPenjualanAnggota.TextBox12.Text = DataGridView1.Item(3, e.RowIndex).Value
                frmPenjualanAnggota.txtIDAnggota.Text = DataGridView1.Item(0, e.RowIndex).Value
            ElseIf Me.Owner Is frmBayarKredit Then
                frmBayarKredit.TextBox1.Text = DataGridView1.Item(3, e.RowIndex).Value
                frmBayarKredit.txtIDAnggota.Text = DataGridView1.Item(0, e.RowIndex).Value
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

    Private Sub frmCariAnggota_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        resetForm()
        refreshGrid()
    End Sub
End Class