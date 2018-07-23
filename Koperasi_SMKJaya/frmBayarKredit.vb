Public Class frmBayarKredit

    Dim sql As String
    Dim simpan As Boolean
    Dim id As String

    Private Sub resetForm()
        resetForm_GLOBAL(Me)
        resetForm_GLOBAL(GroupBox1)
        resetForm_GLOBAL(GroupBox2)

        GroupBox1.Enabled = True
        GroupBox2.Enabled = True

        btnhapus.Enabled = True

        TextBox1.Focus()
    End Sub

    Private Sub refreshGrid()
        sql = "select * from q_kredit where kd_anggota like '%" & txtIDAnggota.Text & "%' and status = 'aktif'"

        fetchData(sql, "q_kredit", DataGridView1, Label5)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)
        If TextBox2.Text <> "" And id <> "" Then
            btnhapus.Enabled = False

            btnsimpan.Enabled = True
            btnbatal.Enabled = True

            GroupBox1.Enabled = False
            GroupBox2.Enabled = True

            TextBox2.Focus()

            simpan = True
        Else
            Eror("Belum ada data yang dipilih!")
            TextBox1.Focus()
        End If
    End Sub

    Private Sub btnhapus_Click(sender As Object, e As EventArgs) Handles btnhapus.Click
        If id <> "" And TextBox2.Text <> "" Then
            If Tanya("Hapus data yang dipilih?") Then
                sql = "delete from tblkredit where kd_kredit = '" & id & "'"

                If execSQL(sql) Then
                    Berhasil("Data berhasil dihapus")
                    resetForm()
                    refreshGrid()
                End If
            End If
        Else
            Eror("Belum ada data yang dipilih!")
            DataGridView1.Focus()
        End If
    End Sub

    Private Sub btnsimpan_Click(sender As Object, e As EventArgs) Handles btnsimpan.Click
        If TextBox2.Text <> "" And TextBox3.Text <> "" Then

            sql = "update tblkredit set status = 'tidak aktif' "
            sql &= "where kd_kredit = " & TextBox2.Text & ""

            If execSQL(sql) Then
                Berhasil("Kredit bulan ke telah terbayar")
                resetForm()
                refreshGrid()
            End If

        Else
            Eror("Lengkapi kembali data anda!")
            TextBox1.Focus()
        End If
    End Sub

    Private Sub btnbatal_Click(sender As Object, e As EventArgs) Handles btnbatal.Click

        Dim proses As String

        proses = "pembayaran kredit"

        If Tanya("Batalkan proses " & proses & "?") Then
            resetForm()
            refreshGrid()
        End If
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Try
            id = DataGridView1.Item(0, e.RowIndex).Value
            TextBox2.Text = DataGridView1.Item(0, e.RowIndex).Value
            TextBox3.Text = DataGridView1.Item(1, e.RowIndex).Value
            TextBox4.Text = DataGridView1.Item(18, e.RowIndex).Value
            TextBox5.Text = DataGridView1.Item(17, e.RowIndex).Value
            DateTimePicker1.Value = DataGridView1.Item(2, e.RowIndex).Value
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        WindowState = FormWindowState.Minimized
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged        
        If TextBox1.Text = "" Then
            DataGridView1.DataSource = Nothing
            Label5.Text = "Jumlah Data : 0"
        End If
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        frmCariAnggota.ShowDialog(Me)
    End Sub

    Private Sub Button3_Click_1(sender As Object, e As EventArgs) Handles Button3.Click
        Close()
    End Sub

    Private Sub txtIDAnggota_TextChanged(sender As Object, e As EventArgs) Handles txtIDAnggota.TextChanged
        refreshGrid()
    End Sub
End Class