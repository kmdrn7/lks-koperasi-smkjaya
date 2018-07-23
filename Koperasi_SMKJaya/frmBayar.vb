Public Class frmBayar

    Dim sql As String
    Dim total As Integer
    Dim bayar As Integer
    Dim kembali As Integer

    Dim tipebayar As String

    Private Sub resetForm()
        resetForm_GLOBAL(GroupBox1)
        resetForm_GLOBAL(GroupBox2)
        ComboBox1.SelectedIndex = 0
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Close()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Close()
    End Sub

    Private Sub frmBayar_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub frmBayar_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        resetForm()
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click

        If ComboBox1.SelectedIndex <> -1 Then
            If tipebayar = "tunai" Then
                If TextBox2.Text <> "" And TextBox3.Text <> "" And TextBox4.Text <> "" And TextBox5.Text <> "" Then
                    Try
                        Convert.ToDouble(TextBox3.Text)
                        Convert.ToDouble(TextBox4.Text)
                        Convert.ToDouble(TextBox5.Text)
                    Catch ex As Exception
                        Eror("Perhatikan kembali jumlah yang anda masukkan")
                        TextBox4.Clear()
                        TextBox4.Focus()
                        Exit Sub
                    End Try

                    sql = "update tblpenjualananggota set total_anggota = " & CInt(TextBox3.Text) & ", jenis_bayar = 'tunai' "
                    sql &= "where kd_penjualan_anggota = '" & frmPenjualanAnggota.TextBox6.Text & "'"
                End If
            ElseIf tipebayar = "potong" Then
                If TextBox2.Text <> "" And TextBox3.Text <> "" Then
                    sql = "insert into tblpotonggaji "
                    sql &= "(kd_penjualan_anggota, kd_anggota, besar_potongan, status) "
                    sql &= "values ('" & frmPenjualanAnggota.TextBox6.Text & "', '" & frmPenjualanAnggota.txtIDAnggota.Text & "', '" & TextBox3.Text & "', 'aktif')"

                    execSQL(sql)

                    sql = "update tblpenjualananggota set total_anggota = " & CInt(TextBox3.Text) & ", jenis_bayar = 'potong' "
                    sql &= "where kd_penjualan_anggota = '" & frmPenjualanAnggota.TextBox6.Text & "'"
                End If
            ElseIf tipebayar = "kredit" Then
                If TextBox2.Text <> "" And TextBox3.Text <> "" Then

                    Dim besarkredit As Double = CInt(TextBox3.Text) / CInt(ComboBox2.Text)

                    For i As Integer = 1 To CInt(ComboBox2.Text)
                        sql = "insert into tblkredit "
                        sql &= "(id_penjualan_anggota, tgl_kredit, besar_kredit, bulan_ke, status) "
                        sql &= "values ('" & frmPenjualanAnggota.TextBox6.Text & "', '" & Format(Today.Date, "MM/dd/yyyy").ToString & "', '" & besarkredit & "', '" & i & "', 'aktif')"

                        execSQL(sql)
                    Next

                    sql = "update tblpenjualananggota set total_anggota = " & CInt(TextBox3.Text) & ", jenis_bayar = 'kredit' "
                    sql &= "where kd_penjualan_anggota = '" & frmPenjualanAnggota.TextBox6.Text & "'"
                End If
            End If

            If ComboBox1.SelectedIndex >= 0 Then
                MessageBox.Show(sql)
                If execSQL(sql) Then
                    Berhasil("Pembayaran telah berhasil dilakukan")
                    frmPenjualanAnggota.resetForm()
                    frmPenjualanAnggota.refreshGrid()
                    Close()
                End If
            End If
        Else
            Eror("Pilih jenis pembayaran terlebih dahulu")
            ComboBox1.Focus()
        End If

    End Sub

    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles TextBox4.TextChanged
        Try
            total = CInt(TextBox3.Text)
            bayar = CInt(TextBox4.Text)

            kembali = bayar - total

            If kembali < 0 Then
                TextBox5.Text = ""
            Else
                TextBox5.Text = kembali
            End If

        Catch ex As Exception
            If TextBox4.Text.Count > 0 Then
                Eror("Perhatikan kembali jumlah yang anda masukkan")
            End If            
            TextBox4.Clear()
            TextBox4.Focus()
        End Try
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.Text = "Tunai" Then
            tipebayar = "tunai"
            Label9.Text = "Kembali"
            Label8.Visible = True
            Label9.Visible = True
            TextBox4.Visible = True
            TextBox5.Visible = True
            Label4.Visible = False
            ComboBox2.Visible = False
        ElseIf ComboBox1.Text = "Potong Gaji" Then
            Label8.Visible = False
            Label9.Visible = False
            TextBox4.Visible = False
            TextBox5.Visible = False
            tipebayar = "potong"
            Label4.Visible = False
            ComboBox2.Visible = False
        ElseIf ComboBox1.Text = "Kredit" Then
            Label9.Text = "Kekurangan"
            tipebayar = "kredit"
            Label8.Visible = True
            Label9.Visible = True
            TextBox4.Visible = True
            TextBox5.Visible = True
            Label4.Visible = True
            ComboBox2.Visible = True
        End If
    End Sub
End Class