﻿Public Class frmAnggota

    Dim sql As String
    Dim simpan As Boolean
    Dim id As String
    Dim jk As String

    Private Sub resetForm()
        fillCombo("select * from tblunitkerja", ComboBox1, "unit_kerja", "kd_unit_kerja")
        resetForm_GLOBAL(Me)
        resetForm_GLOBAL(GroupBox1)
        resetForm_GLOBAL(GroupBox2)

        RadioButton1.Checked = True
        jk = "Pria"

        GroupBox1.Enabled = True
        GroupBox2.Enabled = False

        btnbaru.Enabled = True
        btnubah.Enabled = True
        btnhapus.Enabled = True

        TextBox1.Focus()
    End Sub

    Private Sub refreshGrid()
        sql = "select * from q_anggota where kd_anggota like '%" & TextBox1.Text & "%' or unit_kerja like '%" & TextBox1.Text & "%' or nama_anggota like '%" & TextBox1.Text & "%' or "
        sql &= "tempat_lahir like '%" & TextBox1.Text & "%' or alamat like '%" & TextBox1.Text & "%'"

        fetchData(sql, "q_anggota", DataGridView1, Label5)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnbaru.Click
        btnbaru.Enabled = False
        btnubah.Enabled = False
        btnhapus.Enabled = False

        GroupBox1.Enabled = False
        GroupBox2.Enabled = True

        resetForm_GLOBAL(GroupBox2)

        jk = "Pria"
        RadioButton1.Checked = True

        TextBox2.Text = autoIncrement("select * from tblanggota order by kd_anggota desc", "kd_anggota", "KA")
        TextBox2.Focus()

        simpan = True
    End Sub

    Private Sub btnubah_Click(sender As Object, e As EventArgs) Handles btnubah.Click
        If id <> "" And TextBox2.Text <> "" Then
            btnbaru.Enabled = False
            btnubah.Enabled = False
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
                sql = "delete from tblanggota where kd_anggota = '" & id & "'"

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
        If TextBox2.Text <> "" And TextBox3.Text <> "" And TextBox4.Text <> "" And TextBox5.Text <> "" And TextBox6.Text <> "" And jk <> "" And ComboBox1.SelectedIndex <> -1 And
            ComboBox1.SelectedValue <> "" Then

            If simpan Then

                sql = "insert into tblanggota "
                sql &= "(kd_anggota, kd_unit_kerja, npp, nama_anggota, kelamin, tempat_lahir, tgl_lahir, alamat, tgl_jadi_anggota) "
                sql &= "values ('" & TextBox2.Text & "', '" & ComboBox1.SelectedValue & "', " & TextBox3.Text & ", '" & TextBox4.Text & "', '" & jk & "', '" & TextBox5.Text & "', '" & Format(DateTimePicker1.Value, "MM/dd/yyyy").ToString & "', '" & TextBox6.Text & "', '" & Format(DateTimePicker2.Value, "MM/dd/yyyy").ToString & "')"

                If execSQL(sql) Then
                    Berhasil("Data telah berhasil disimpan")
                    resetForm()
                    refreshGrid()
                End If

            Else

                sql = "update tblanggota set "
                sql &= "kd_anggota = '" & TextBox2.Text & "', kd_unit_kerja = '" & ComboBox1.SelectedValue & "', npp = " & TextBox3.Text & ", nama_anggota = '" & TextBox4.Text & "', kelamin = '" & jk & "', tempat_lahir = '" & TextBox5.Text & "', tgl_lahir = '" & Format(DateTimePicker1.Value, "MM/dd/yyyy").ToString & "', alamat = '" & TextBox6.Text & "', tgl_jadi_anggota = '" & Format(DateTimePicker2.Value, "MM/dd/yyyy").ToString & "' "
                sql &= "where kd_anggota = '" & id & "'"

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
            ComboBox1.SelectedValue = DataGridView1.Item(1, e.RowIndex).Value
            TextBox3.Text = DataGridView1.Item(2, e.RowIndex).Value
            TextBox4.Text = DataGridView1.Item(3, e.RowIndex).Value

            If DataGridView1.Item(4, e.RowIndex).Value = "Pria" Then
                jk = "Pria"
                RadioButton1.Checked = True
            Else
                jk = "Perempuan"
                RadioButton2.Checked = True
            End If

            TextBox5.Text = DataGridView1.Item(5, e.RowIndex).Value
            DateTimePicker1.Value = DataGridView1.Item(6, e.RowIndex).Value
            TextBox6.Text = DataGridView1.Item(7, e.RowIndex).Value
            DateTimePicker2.Value = DataGridView1.Item(8, e.RowIndex).Value
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

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked Then
            jk = "Pria"            
        End If
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged
        If RadioButton2.Checked Then
            jk = "Perempuan"
        End If
    End Sub

    Private Sub frmAnggota_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        resetForm()
        refreshGrid()
    End Sub

    Private Sub Button3_Click_1(sender As Object, e As EventArgs) Handles Button3.Click
        frmCariUnitKerja.ShowDialog(Me)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Close()
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        DateTimePicker2.Value = Today.Date
    End Sub
End Class