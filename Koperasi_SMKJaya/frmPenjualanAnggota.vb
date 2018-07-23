Public Class frmPenjualanAnggota

    Dim sql As String
    Dim id As String
    Dim simpan As String
    Dim kd_pengguna As String

    Dim bayar
    Dim kembali
    Dim GrandTotal

    Public Sub resetForm()
        resetForm_GLOBAL(Me)
        resetForm_GLOBAL(GroupBox1)
        resetForm_GLOBAL(GroupBox2)
        resetForm_GLOBAL(GroupBox3)

        TextBox1.Text = 0

        TextBox8.Text = Username

        kd_pengguna = getValue("select * from tblpengguna where username = '" & Username & "'", "kd_pengguna")

        TextBox6.Text = myIncrement("select * from tblpenjualananggota", "PA")
        TextBox6.Text = autoIncrement("select * from tblpenjualananggota order by kd_penjualan_anggota desc", "kd_penjualan_anggota", "PA")

        TextBox7.Text = "Anggota"
    End Sub

    Public Sub refreshGrid()
        sql = "select * from q_detail_anggota where kd_penjualan_anggota = '" & TextBox6.Text & "'"

        fetchData(sql, "q_detail_anggota", DataGridView1)

        hitungJumlah()
    End Sub

    Private Sub hitungJumlah()
        Dim total As Integer
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            total = total + (DataGridView1.Item(4, i).Value * DataGridView1.Item(5, i).Value)
        Next
        TextBox1.Text = total.ToString
    End Sub

    Private Sub refreshBarang()
        sql = "select * from tblbarang where kd_barang = '" & TextBox2.Text & "'"

        TextBox3.Text = getValue(sql, "nama_barang")
        TextBox4.Text = getValue(sql, "harga_jual")
        TextBox11.Text = getValue(sql, "stok_barang")

        If TextBox3.Text <> "" Then
            TextBox5.Text = 1
        End If
    End Sub

    '====================== FAKTUR =======================

    Private Function issetFaktur()
        If rowCount("select * from tblpenjualananggota where kd_penjualan_anggota = '" & TextBox6.Text & "'") > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub insertFaktur()        
        sql = "insert into tblpenjualananggota (kd_penjualan_anggota, kd_anggota, tgl_transaksi_anggota, total_anggota) "
        sql &= "values ('" & TextBox6.Text & "', '" & txtIDAnggota.Text & "', '" & Format(DateTimePicker1.Value, "MM/dd/yyyy").ToString & "', 0)"

        execSQL(sql)
    End Sub

    Private Sub insertBarang()
        sql = "insert into tbldetailanggota (kd_detail_anggota, kd_barang, kd_penjualan_anggota, kd_pengguna, harga_total, jumlah_barang_anggota, sub_total_anggota) "
        sql &= "values ('" & myIncrement("select * from tbldetailanggota", "DA") & "', '" & TextBox2.Text & "', '" & TextBox6.Text & "', '" & kd_pengguna & "',"
        sql &= "" & TextBox4.Text & ", " & TextBox5.Text & ", " & CInt(TextBox4.Text) * CInt(TextBox5.Text) & ")"

        execSQL(sql)
    End Sub

    Private Sub kurangiStok()
        Dim stok As Integer = CInt(TextBox11.Text) - CInt(TextBox5.Text)

        sql = "update tblbarang set stok_barang = " & stok & " "
        sql &= "where kd_barang = '" & TextBox2.Text & "'"

        execSQL(sql)
    End Sub

    Private Sub tambahStok()
        Dim stok As Integer = CInt(TextBox11.Text) + CInt(TextBox5.Text)

        sql = "update tblbarang set stok_barang = " & stok & " "
        sql &= "where kd_barang = '" & TextBox2.Text & "'"

        execSQL(sql)
    End Sub

    '====================== FAKTUR =======================

    Private Sub GroupBox2_Enter(sender As Object, e As EventArgs) Handles GroupBox2.Enter

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        WindowState = FormWindowState.Minimized
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        DateTimePicker1.Value = Today.Date
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        frmCariBarang.Show(Me)
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        refreshBarang()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click

        If txtIDAnggota.Text = "" Then
            Eror("Pilih anggota terlebih dahulu")
            TextBox12.Focus()
            Exit Sub
        End If

        Dim dbl As Double
        Try
            dbl = CDbl(TextBox5.Text)
        Catch ex As Exception

        End Try

        If TextBox2.Text <> "" And TextBox3.Text <> "" And TextBox5.Text <> "" And dbl > 0 Then

            If dbl > CDbl(TextBox11.Text) Then
                Eror("Tidak bisa melebihi batas stok")
                Exit Sub
            End If

            Try
                Convert.ToInt16(TextBox5.Text)
            Catch ex As Exception
                MessageBox.Show("Perhatikan kembali jumlah barang anda")
                TextBox5.Clear()
                TextBox5.Focus()
            End Try

            ' jika sudah masuk maka tidak dimasukkan lagi
            If Not issetFaktur() Then
                insertFaktur()
                insertBarang()
                kurangiStok()
                resetForm_GLOBAL(GroupBox2)
                TextBox2.Focus()
                refreshGrid()
            Else
                insertBarang()
                kurangiStok()
                resetForm_GLOBAL(GroupBox2)
                TextBox2.Focus()
                refreshGrid()
            End If

        Else
            Eror("Ada data yang kosong!")
            TextBox2.Focus()
        End If
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Try
            id = DataGridView1.Item(0, e.RowIndex).Value
            TextBox2.Text = DataGridView1.Item(1, e.RowIndex).Value
            TextBox3.Text = DataGridView1.Item(10, e.RowIndex).Value
            TextBox4.Text = DataGridView1.Item(12, e.RowIndex).Value
            TextBox5.Text = DataGridView1.Item(5, e.RowIndex).Value
            TextBox11.Text = DataGridView1.Item(13, e.RowIndex).Value
        Catch ex As Exception

        End Try
    End Sub

    Private Sub TextBox5_TextChanged(sender As Object, e As EventArgs) Handles TextBox5.TextChanged

        If TextBox5.Text <> "" Then
            Try
                Dim hrg As Integer = CInt(TextBox4.Text)
                Dim jml As Integer = CInt(TextBox5.Text)

                Dim total As Integer = hrg * jml

                If total < 0 Then
                    TextBox1.Text = ""
                Else
                    TextBox1.Text = hrg * jml
                End If

            Catch ex As Exception
                TextBox1.Text = "jumlah salah"
            End Try

        Else
            TextBox1.Clear()
        End If

    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click

        If id <> "" And txtID.Text <> "" Then
            sql = "delete from tbldetailanggota where kd_detail_anggota = '" & id & "'"

            If execSQL(sql) Then
                Berhasil("Barang berhasil dihapus")
                refreshGrid()
                resetForm_GLOBAL(GroupBox2)
                TextBox2.Focus()
            End If
        Else
            Eror("Belum ada data yang dipilih!")
            TextBox2.Focus()
        End If

    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click

        If DataGridView1.Rows.Count < 1 Then
            Eror("Tidak bisa dilakukan pembatalan karena belum ada transaksi!")
            Exit Sub
        End If

        If Tanya("Batalkan transaksi? data yang sudah ditambahkan akan dihapus") Then

            For i As Integer = 0 To DataGridView1.Rows.Count - 1
                Dim idbarang As String = DataGridView1.Item(1, i).Value
                Dim jumlahbarang As Integer = DataGridView1.Item(5, i).Value

                Dim jumlahawal As Integer = getValue("select * from tblbarang where kd_barang = '" & idbarang & "'", "stok_barang")

                sql = "update tblbarang set stok_barang = " & jumlahbarang + jumlahawal & " "
                sql &= "where kd_barang = '" & idbarang & "'"

                execSQL(sql)
            Next

            sql = "delete from tbldetailanggota where kd_penjualan_anggota = '" & TextBox6.Text & "'"
            execSQL(sql)

            sql = "delete from tblpenjualananggota where kd_penjualan_anggota = '" & TextBox6.Text & "'"

            If execSQL(sql) Then
                resetForm()
                refreshGrid()
                refreshBarang()
            End If

        End If

    End Sub
    
    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        If Tanya("Reset akan membatalkan transaksi? data yang sudah ditambahkan akan dihapus") Then

            For i As Integer = 0 To DataGridView1.Rows.Count - 1
                Dim idbarang As String = DataGridView1.Item(1, i).Value
                Dim jumlahbarang As Integer = DataGridView1.Item(5, i).Value

                Dim jumlahawal As Integer = getValue("select * from tblbarang where kd_barang = '" & idbarang & "'", "stok_barang")

                sql = "update tblbarang set stok_barang = " & jumlahbarang + jumlahawal & " "
                sql &= "where kd_barang = '" & idbarang & "'"

                execSQL(sql)
            Next

            sql = "delete from tbldetailanggota where kd_penjualan_anggota = '" & TextBox6.Text & "'"
            execSQL(sql)

            sql = "delete from tblpenjualananggota where kd_penjualan_anggota = '" & TextBox6.Text & "'"

            If execSQL(sql) Then
                resetForm()
                refreshGrid()
                refreshBarang()
            End If

        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Close()
    End Sub

    Private Sub frmPenjualanAnggota_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        resetForm()
        refreshGrid()
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        frmCariAnggota.ShowDialog(Me)
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        If DataGridView1.Rows.Count < 1 Then
            Eror("Belum ada transaksi yang bisa dibayar")
        Else
            Try
                frmBayar.TextBox2.Text = TextBox12.Text
                frmBayar.TextBox3.Text = CInt(TextBox1.Text)
                frmBayar.ShowDialog(Me)
            Catch ex As Exception
                frmBayar.Dispose()
            End Try
        End If
    End Sub
End Class