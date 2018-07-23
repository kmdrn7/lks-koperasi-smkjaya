Public Class frmGajian

    Dim sql As String
    Dim simpan As Boolean
    Dim id As String
    Dim besargajian As Integer

    Private Sub resetForm()
        resetForm_GLOBAL(Me)
        resetForm_GLOBAL(GroupBox1)
        resetForm_GLOBAL(GroupBox2)

        GroupBox1.Enabled = True
        GroupBox2.Enabled = False

        btnbaru.Enabled = True
        btnhapus.Enabled = True

        TextBox1.Focus()
    End Sub

    Private Sub refreshGrid()
        sql = "select * from q_gajian where kd_gajian like '%" & TextBox1.Text & "%' or nama_anggota like '%" & TextBox1.Text & "%' or unit_kerja like '%" & TextBox1.Text & "%' or "
        sql &= "tgl_gajian like '%" & TextBox1.Text & "%'"

        fetchData(sql, "q_gajian", DataGridView1, Label5)
    End Sub

    Private Sub cekPotonganGaji()
        sql = "select * from q_potonggaji where kd_anggota = '" & txtIDAnggota.Text & "' and status = 'aktif'"

        If rowCount(sql) > 0 Then
            Dim potongan As Integer = getValue(sql, "besar_potongan")

            besargajian = CInt(TextBox4.Text) - potongan

            sql = "update tblpotonggaji set status = 'tidak aktif' where kd_anggota = '" & txtIDAnggota.Text & "'"

            If execSQL(sql) Then
                Berhasil("Gajian dipotong sebesar " & potongan & " untuk membayar kredit")
            End If

        Else
            besargajian = CInt(TextBox4.Text)
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnbaru.Click
        btnbaru.Enabled = False
        btnhapus.Enabled = False

        GroupBox1.Enabled = False
        GroupBox2.Enabled = True

        resetForm_GLOBAL(GroupBox2)

        TextBox2.Text = autoIncrement("select * from tblgajian order by kd_gajian desc", "kd_gajian", "KG")

        TextBox2.Focus()

        simpan = True
    End Sub

    Private Sub btnubah_Click(sender As Object, e As EventArgs)
        If id <> "" And TextBox2.Text <> "" Then
            btnbaru.Enabled = False
            btnhapus.Enabled = False

            GroupBox1.Enabled = False
            GroupBox2.Enabled = True

            TextBox2.Focus()

            simpan = False
        Else
            Eror("Belum ada data yang dipilih!")
            DataGridView1.Focus()
        End If
    End Sub

    Private Sub btnhapus_Click(sender As Object, e As EventArgs) Handles btnhapus.Click
        If id <> "" And TextBox2.Text <> "" Then
            If Tanya("Hapus data yang dipilih?") Then
                sql = "delete from tblgajian where kd_gajian = '" & id & "'"

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
        If TextBox2.Text <> "" And TextBox3.Text <> "" And TextBox4.Text <> "" Then

            If simpan Then

                cekPotonganGaji()

                sql = "insert into tblgajian "
                sql &= "(kd_gajian, kd_anggota, tgl_gajian, besar_gajian) "
                sql &= "values ('" & TextBox2.Text & "', '" & txtIDAnggota.Text & "', '" & Format(DateTimePicker1.Value, "MM/dd/yyyy").ToString & "', '" & besargajian & "')"

                If execSQL(sql) Then
                    Berhasil("Data telah berhasil disimpan")
                    resetForm()
                    refreshGrid()
                End If

            Else

                sql = "update tblgajian set "
                sql &= "kd_gajian = '" & TextBox2.Text & "', kd_anggota = '" & txtIDAnggota.Text & "', tgl_gajian = '" & Format(DateTimePicker1.Value, "MM/dd/yyyy").ToString & "', besar_gajian = '" & TextBox4.Text & "' "
                sql &= "where kd_gajian = '" & id & "'"

                If execSQL(sql) Then
                    Berhasil("Data telah berhasil diubah")
                    resetForm()
                    refreshGrid()
                End If

            End If

        Else
            Eror("Lengkapi kembali data anda!")
            TextBox1.Focus()
        End If
    End Sub

    Private Sub btnbatal_Click(sender As Object, e As EventArgs) Handles btnbatal.Click

        Dim proses As String

        If simpan Then
            proses = "penambahan data baru"
        Else
            proses = "pengubahan data"
        End If

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
            DateTimePicker1.Value = DataGridView1.Item(2, e.RowIndex).Value
            TextBox4.Text = DataGridView1.Item(3, e.RowIndex).Value
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
        refreshGrid()
    End Sub

    Private Sub Button3_Click_1(sender As Object, e As EventArgs) Handles Button3.Click
        Close()
    End Sub

    Private Sub frmGajian_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        resetForm()
        refreshGrid()
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        frmCariAnggota.ShowDialog(Me)
    End Sub
End Class