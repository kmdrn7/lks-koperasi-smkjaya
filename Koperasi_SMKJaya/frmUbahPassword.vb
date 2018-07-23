Public Class frmUbahPassword

    Dim sql As String
    Dim id As String

    Private Sub resetForm()
        resetForm_GLOBAL(GroupBox1)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If TextBox1.Text <> "" And TextBox2.Text <> "" Then
            If Tanya("Ubah password anda?") Then
                Dim sql As String = "select * from tblpengguna where username = '" & Username & "'"

                Dim id As String = getValue(sql, "kd_pengguna")
                Dim pass As String = getValue(sql, "password")

                If TextBox1.Text <> pass Then
                    Eror("Password lama anda salah!")
                    resetForm()
                    Exit Sub
                End If

                sql = "update tblpengguna set password = '" & TextBox2.Text & "' "
                sql &= "where kd_pengguna = '" & id & "'"

                If execSQL(sql) Then
                    Berhasil("Password anda berhasil dirubah")
                    Close()
                End If
            End If
        Else
            Eror("Lengkapi kemblai data anda!")
        End If
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
End Class