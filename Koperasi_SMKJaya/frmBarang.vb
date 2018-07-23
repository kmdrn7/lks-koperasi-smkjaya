﻿Public Class frmBarang

    Dim sql As String
    Dim simpan As Boolean
    Dim id As String

    Private Sub resetForm()
        fillCombo("select * from tbljenisbarang", ComboBox1, "jenis_barang", "kd_jenis_barang")
        resetForm_GLOBAL(Me)
        resetForm_GLOBAL(GroupBox1)
        resetForm_GLOBAL(GroupBox2)

        GroupBox1.Enabled = True
        GroupBox2.Enabled = False

        btnbaru.Enabled = True
        btnubah.Enabled = True
        btnhapus.Enabled = True

        TextBox1.Focus()
    End Sub

    Private Sub refreshGrid()
        sql = "select * from q_jenisbarang where kd_jenis_barang like '%" & TextBox1.Text & "%' or nama_barang like '%" & TextBox1.Text & "%' or jenis_barang like '%" & TextBox1.Text & "%' or "
        sql &= "harga_beli like '%" & TextBox1.Text & "%' or harga_jual like '%" & TextBox1.Text & "%' or stok_barang like '%" & TextBox1.Text & "%'"

        fetchData(sql, "q_jenisbarang", DataGridView1, Label5)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnbaru.Click
        btnbaru.Enabled = False
        btnubah.Enabled = False
        btnhapus.Enabled = False

        GroupBox1.Enabled = False
        GroupBox2.Enabled = True

        resetForm_GLOBAL(GroupBox2)

        TextBox2.Text = autoIncrement("select * from tblbarang order by kd_barang desc", "kd_barang", "KB")
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
                sql = "delete from tblbarang where kd_barang = '" & id & "'"

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
        If TextBox2.Text <> "" And TextBox3.Text <> "" And TextBox4.Text <> "" And TextBox5.Text <> "" And TextBox6.Text <> "" And ComboBox1.SelectedIndex <> -1 And
            ComboBox1.SelectedValue <> "" Then

            If simpan Then

                sql = "insert into tblbarang "
                sql &= "(kd_barang, kd_jenis_barang, nama_barang, harga_beli, harga_jual, stok_barang, keterangan_barang) "
                sql &= "values ('" & TextBox2.Text & "', '" & ComboBox1.SelectedValue & "', '" & TextBox3.Text & "', " & TextBox4.Text & ", " & TextBox5.Text & ", " & TextBox6.Text & ", '" & TextBox7.Text & "')"

                If execSQL(sql) Then
                    Berhasil("Data telah berhasil disimpan")
                    resetForm()
                    refreshGrid()
                End If

            Else

                sql = "update tblbarang set "
                sql &= "kd_barang = '" & TextBox2.Text & "', kd_jenis_barang = '" & ComboBox1.SelectedValue & "', nama_barang = '" & TextBox3.Text & "', harga_beli = " & TextBox4.Text & ", harga_jual = " & TextBox5.Text & ", stok_barang = " & TextBox6.Text & ", keterangan_barang = '" & TextBox7.Text & "' "
                sql &= "where kd_barang = '" & id & "'"

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
            TextBox5.Text = DataGridView1.Item(4, e.RowIndex).Value
            TextBox6.Text = DataGridView1.Item(5, e.RowIndex).Value
            If IsDBNull(DataGridView1.Item(6, e.RowIndex).Value) Then
                TextBox7.Clear()
            Else
                TextBox7.Text = DataGridView1.Item(6, e.RowIndex).Value
            End If            
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

    Private Sub frmBarang_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        resetForm()
        refreshGrid()
    End Sub

    Private Sub Button3_Click_1(sender As Object, e As EventArgs) Handles Button3.Click
        frmCariJenisBarang.ShowDialog(Me)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Close()
    End Sub
End Class