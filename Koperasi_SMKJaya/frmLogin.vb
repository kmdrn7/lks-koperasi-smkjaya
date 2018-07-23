Public Class frmLogin

    Dim sql As String

    Private Sub resetForm()
        resetForm_GLOBAL(GroupBox1)
        TextBox1.Focus()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        WindowState = FormWindowState.Minimized
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If TextBox1.Text <> "" And TextBox2.Text <> "" Then

            sql = "select * from tblpengguna where username = '" & TextBox1.Text & "' and password = '" & TextBox2.Text & "'"

            If rowCount(sql) > 0 Then
                Dim user As String = getValue(sql, "nama_pengguna")
                level = getValue(sql, "level")
                Username = getValue(sql, "username")
                frmMain.ToolStripStatusLabel3.Text = "User Aktif : [" & Username & "]"
                frmMain.ToolStripStatusLabel4.Text = "Group Aktif : [" & level & "]"
                Berhasil("LOGIN TELAH BERHASIL" & vbNewLine & vbNewLine & "Selamat datang, " & user)
                isLogin = True
                frmMain.LoginToolStripMenuItem.Text = "Logout"
                Close()
            Else
                Label5.Visible = True
                TextBox1.Clear()
                TextBox2.Clear()
                TextBox1.Focus()
            End If
        Else
            MessageBox.Show("Lengkapi kembali identitas anda")
            TextBox1.Focus()
        End If
    End Sub

    Private Sub TextBox1_KeyUp(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyUp

    End Sub

    Private Sub frmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        resetForm()
        Label5.Visible = False        
    End Sub

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        Label5.Visible = False
    End Sub
End Class