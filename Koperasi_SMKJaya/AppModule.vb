Imports System.Data
Imports System.Data.SqlClient
Imports System.Text

Module AppModule
    Public conn As New SqlConnection
    Public path As String = "data source = DESKTOP-51456GO\SQLEXPRESS; initial catalog = koperasi_smk; user = sa; password = andika12345"
    'Public path As String = "Data Source=.\SQLEXPRESS;AttachDbFilename=I:\LKS_ITSA\4. NGANJUK 2016\New folder\Sesi 4\koperasi_smk.mdf;Integrated Security=True;Connect Timeout=30;User Instance=False"
    Public CMD As New SqlCommand
    Public DA As New SqlDataAdapter
    Public DS As New DataSet
    Public DT As New DataTable
    Public DR As SqlDataReader

    Public Username As String
    Public isConnect As Boolean
    Public isLogin As Boolean
    Public level As String

    Public GrandTotal As Double = 0

    Public Sub Koneksi()
        If conn.State = ConnectionState.Closed Then
            Try
                conn.ConnectionString = path
                conn.Open()
                isConnect = True
            Catch ex As Exception
                MessageBox.Show("Error koneksi ke database " & vbNewLine & vbNewLine & ex.Message)
                isConnect = False
            End Try
        End If
    End Sub

    Public Function fetchData(ByVal sql As String, tblname As String, grid As DataGridView, Optional jumlah As Label = Nothing)
        Koneksi()
        Dim DS As New DataSet

        Try
            CMD.Connection = conn
            CMD.CommandType = CommandType.Text
            CMD.CommandText = sql

            DA.SelectCommand = CMD
            DS.Clear()
            DA.Fill(DS, tblname)

            grid.DataSource = DS.Tables(tblname)

            If jumlah IsNot Nothing Then
                jumlah.Text = "Jumlah Data : " & grid.RowCount
            Else
                GrandTotal = 0

                If tblname = "q_detail_umum" Or tblname = "q_detail_anggota" Then
                    For i As Integer = 0 To grid.Rows.Count - 1
                        GrandTotal = GrandTotal + (grid.Item(4, i).Value * grid.Item(5, i).Value)
                    Next
                Else
                    MessageBox.Show("salah")
                End If

            End If

            CMD.Dispose()
            conn.Close()
            DA.Dispose()

            Return True
        Catch ex As Exception
            MessageBox.Show("Error saat fetchdata" & vbNewLine & vbNewLine & ex.Message)
            Return False
            CMD.Dispose()
            conn.Close()
            DA.Dispose()
        Finally
            CMD.Dispose()
            conn.Close()
            DA.Dispose()
        End Try
    End Function

    Public Function execSQL(ByVal sql As String)
        Koneksi()

        Try
            CMD.Connection = conn
            CMD.CommandType = CommandType.Text
            CMD.CommandText = sql

            CMD.ExecuteNonQuery()

            CMD.Dispose()
            conn.Close()

            Return True
        Catch ex As Exception
            MessageBox.Show("Error saat execSQL" & vbNewLine & vbNewLine & ex.Message)
            Return False
            CMD.Dispose()
            conn.Close()
        Finally
            CMD.Dispose()
            conn.Close()
        End Try
    End Function

    Public Function rowCount(ByVal sql As String)
        Koneksi()        

        Try
            CMD.Connection = conn
            CMD.CommandType = CommandType.Text
            CMD.CommandText = sql

            DT.Clear()
            DA.SelectCommand = CMD
            DA.Fill(DT)

            Return DT.Rows.Count

            CMD.Dispose()
            conn.Close()
            DA.Dispose()
        Catch ex As Exception
            MessageBox.Show("Error saat rowcount" & vbNewLine & vbNewLine & ex.Message)
            Return 0
            CMD.Dispose()
            conn.Close()
            DA.Dispose()
        Finally
            CMD.Dispose()
            conn.Close()
            DA.Dispose()
        End Try
    End Function

    Public Function fillCombo(ByVal sql As String, cb As ComboBox, display As String, value As String)
        Koneksi()
        Dim DT As New DataTable

        Try
            CMD.Connection = conn
            CMD.CommandType = CommandType.Text
            CMD.CommandText = sql

            cb.DataSource = Nothing
            cb.DisplayMember = Nothing
            cb.ValueMember = Nothing

            DT.Clear()
            DA.SelectCommand = CMD
            DA.Fill(DT)

            cb.DataSource = DT
            cb.DisplayMember = display
            cb.ValueMember = value

            CMD.Dispose()
            conn.Close()
            DA.Dispose()
            Return True
        Catch ex As Exception
            MessageBox.Show("Error saat fillcombo" & vbNewLine & vbNewLine & ex.Message)
            Return False
        Finally
            CMD.Dispose()
            conn.Close()
            DA.Dispose()
        End Try
    End Function

    Public Function getValue(ByVal sql As String, colname As String)
        Koneksi()

        Try
            CMD.Connection = conn
            CMD.CommandType = CommandType.Text
            CMD.CommandText = sql

            DR = CMD.ExecuteReader
            DR.Read()

            If DR.HasRows Then
                Return DR.Item(colname)
            Else
                Return ""
            End If

            CMD.Dispose()
            DR.Close()
            conn.Close()
        Catch ex As Exception
            MessageBox.Show("Error saat getvalue" & vbNewLine & vbNewLine & ex.Message)
            Return ""
            CMD.Dispose()
            DR.Close()
            conn.Close()
        Finally
            CMD.Dispose()
            DR.Close()
            conn.Close()
        End Try
    End Function

    Public Function myIncrement(ByVal sql As String, index As String)
        Koneksi()

        Try
            CMD.Connection = conn
            CMD.CommandType = CommandType.Text
            CMD.CommandText = sql

            DT.Clear()
            DA.SelectCommand = CMD
            DA.Fill(DT)

            Return index & Format(DT.Rows.Count + 1, "D4")

            CMD.Dispose()
            conn.Close()
            DA.Dispose()
        Catch ex As Exception
            MessageBox.Show("Error saat generate increment" & vbNewLine & vbNewLine & ex.Message)
            Return 0
            CMD.Dispose()
            conn.Close()
            DA.Dispose()
        Finally
            CMD.Dispose()
            conn.Close()
            DA.Dispose()
        End Try
    End Function

    Public Function autoIncrement(ByVal sql As String, index As String, kode As String)
        Koneksi()

        Try
            CMD.Connection = conn
            CMD.CommandType = CommandType.Text
            CMD.CommandText = sql

            DR = CMD.ExecuteReader
            DR.Read()

            Dim txt As String

            If DR.HasRows Then
                txt = DR.Item(index)
            Else
                txt = "000000"
            End If

            Dim number As Integer = txt.Substring(2, 4)

            Dim result As String = kode & Format(number + 1, "D4")

            CMD.Dispose()
            conn.Close()
            DR.Close()

            Return result
        Catch ex As Exception
            MessageBox.Show("Error saat generate increment" & vbNewLine & vbNewLine & ex.Message)
            Return ""
            CMD.Dispose()
            conn.Close()
            DR.Close()
        Finally
            CMD.Dispose()
            conn.Close()
            DR.Close()
        End Try

    End Function

    '===========================  ADDITIONAL FUNCTION by me As Andikahmadr ====================================

    Public Function Tanya(ByVal txt As String)
        If MessageBox.Show(txt, "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = vbYes Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub Eror(ByVal txt As String)
        MessageBox.Show(txt, "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Public Sub Berhasil(ByVal txt As String)
        MessageBox.Show(txt, "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Public Sub resetForm_GLOBAL(ByVal control As Control)
        For Each ct As Control In control.Controls
            If TypeOf ct Is TextBox Then
                DirectCast(ct, TextBox).Clear()
            End If

            If TypeOf ct Is ComboBox Then
                DirectCast(ct, ComboBox).SelectedIndex = -1
            End If

            If TypeOf ct Is DateTimePicker Then
                DirectCast(ct, DateTimePicker).Value = Today.Date
            End If

            If TypeOf ct Is RadioButton Then
                DirectCast(ct, RadioButton).Checked = False
            End If
        Next
    End Sub
End Module
