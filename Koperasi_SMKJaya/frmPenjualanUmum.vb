Public Class frmPenjualanUmum

    Dim sql As String
    Dim id As String
    Dim simpan As String
    Dim kd_pengguna As String

    Dim bayar
    Dim kembali
    Dim GrandTotal

    Private Sub resetForm()
        resetForm_GLOBAL(Me)
        resetForm_GLOBAL(GroupBox1)
        resetForm_GLOBAL(GroupBox2)
        resetForm_GLOBAL(GroupBox3)
        resetForm_GLOBAL(GroupBox4)

        TextBox1.Text = 0

        TextBox8.Text = Username

        kd_pengguna = getValue("select * from tblpengguna where username = '" & Username & "'", "kd_pengguna")

        TextBox6.Text = myIncrement("select * from tblpenjualanumum", "PU")
        TextBox6.Text = autoIncrement("select * from tblpenjualanumum order by kd_penjualan_umum desc", "kd_penjualan_umum", "PU")
        TextBox7.Text = "Umum"
    End Sub

    Private Sub refreshGrid()
        sql = "select * from q_detail_umum where kd_penjualan_umum = '" & TextBox6.Text & "'"

        fetchData(sql, "q_detail_umum", DataGridView1)

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
        If rowCount("select * from tblpenjualanumum where kd_penjualan_umum = '" & TextBox6.Text & "'") > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub insertFaktur()
        sql = "insert into tblpenjualanumum (kd_penjualan_umum, tgl_transaksi_umum, total_umum) "
        sql &= "values ('" & TextBox6.Text & "', '" & Format(DateTimePicker1.Value, "MM/dd/yyyy").ToString & "', 0)"

        execSQL(sql)
    End Sub

    Private Sub insertBarang()
        sql = "insert into tbldetailumum (kd_detail_umum, kd_barang, kd_penjualan_umum, kd_pengguna, harga_umum, jumlah_barang_umum, sub_total_umum) "
        sql &= "values ('" & myIncrement("select * from tbldetailumum", "DU") & "', '" & TextBox2.Text & "', '" & TextBox6.Text & "', '-',"
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
        'txtID.Text <> ""

        If id <> "" And TextBox2.Text <> "" And TextBox3.Text <> "" Then
            sql = "delete from tbldetailumum where kd_detail_umum = '" & id & "'"

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

            sql = "delete from tbldetailumum where kd_penjualan_umum = '" & TextBox6.Text & "'"
            execSQL(sql)

            sql = "delete from tblpenjualanumum where kd_penjualan_umum = '" & TextBox6.Text & "'"

            If execSQL(sql) Then
                resetForm()
                refreshGrid()
                refreshBarang()
            End If

        End If

    End Sub

    Private Sub TextBox9_TextChanged(sender As Object, e As EventArgs) Handles TextBox9.TextChanged

        If TextBox1.Text <> "0" Or TextBox1.Text <> "" Then
            Try
                Dim bayar As Integer = CInt(TextBox9.Text)
                Dim jml As Integer = CInt(TextBox1.Text)

                If jml <= 0 Then
                    Eror("belum ada transaksi")
                    TextBox9.Clear()
                    TextBox2.Focus()
                    Exit Sub
                End If

                Dim hasil = bayar - jml

                If hasil <= 0 Then
                    TextBox10.Text = "-"
                Else
                    TextBox10.Text = hasil
                End If

            Catch ex As Exception

            End Try
        Else
            Eror("Isikan dahulu data pembelian!")
        End If

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        Try
            Convert.ToDouble(TextBox9.Text)
        Catch ex As Exception
            Eror("Uang yang anda masukkan salah")
            TextBox9.Clear()
            TextBox9.Focus()
            Exit Sub
        End Try

        If TextBox10.Text <> "-" And TextBox1.Text <> "" And DataGridView1.Rows.Count > 0 And TextBox9.Text <> "" Then
            sql = "update tblpenjualanumum set total_umum = " & CInt(TextBox1.Text) & " "
            sql &= "where kd_penjualan_umum = '" & TextBox6.Text & "'"

            If execSQL(sql) Then
                MessageBox.Show("Data berhasil dicatat")
                resetForm()
                refreshGrid()
                refreshBarang()
                TextBox2.Focus()
            End If
        Else

            If DataGridView1.Rows.Count < 1 Then
                Eror("Belum ada belanjaan")
            ElseIf TextBox10.Text = "-" Then
                Eror("Uang yang anda masukkan kurang")
            Else
                Eror("Error saat simpan, data anda belum lengkap")
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

            sql = "delete from tbldetailumum where kd_penjualan_umum = '" & TextBox6.Text & "'"
            execSQL(sql)

            sql = "delete from tblpenjualanumum where kd_penjualan_umum = '" & TextBox6.Text & "'"

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

    Private Sub frmPenjualanUmum_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        resetForm()
        refreshGrid()
    End Sub

    Private Sub TextBox6_TextChanged(sender As Object, e As EventArgs) Handles TextBox6.TextChanged

    End Sub
End Class