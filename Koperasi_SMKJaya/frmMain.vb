Public Class frmMain

    Private Sub resetForm()
        ToolStripStatusLabel1.Text = Today.Date.ToString("dddd, dd MMMM yyyy")        
        ToolStripStatusLabel3.Text = "User Aktif : [belum_ada]"
        ToolStripStatusLabel4.Text = "Group Aktif : [belum_ada]"
    End Sub

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Koneksi()

        If Not isConnect Then
            Close()
        End If

        Timer1.Start()

        resetForm()

        frmLogin.ShowDialog(Me)

        If Not isLogin Then            
            MasterToolStripMenuItem.Visible = False
            TransaksiToolStripMenuItem.Visible = False
            LaporanToolStripMenuItem.Visible = False            
        Else
            MasterToolStripMenuItem.Visible = True
            TransaksiToolStripMenuItem.Visible = True
            LaporanToolStripMenuItem.Visible = True
        End If
    End Sub

    Private Sub frmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If isConnect Then
            If Tanya("Keluar dari aplikasi?") Then
                e.Cancel = False
            Else
                e.Cancel = True
            End If
        Else
            e.Cancel = False
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        WindowState = FormWindowState.Minimized
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Close()
    End Sub

    Private Sub LoginToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoginToolStripMenuItem.Click

        If LoginToolStripMenuItem.Text = "Login" Then
            frmLogin.ShowDialog(Me)
        Else
            If Tanya("Apakah anda ingin logout?") Then
                isLogin = False
                Username = ""
                level = ""
                LoginToolStripMenuItem.Text = "Login"
                ToolStripStatusLabel3.Text = "User Aktif : [tidak ada]"
                ToolStripStatusLabel4.Text = "Group Aktif : [tidak ada]"
                MasterToolStripMenuItem.Visible = False
                TransaksiToolStripMenuItem.Visible = False
                LaporanToolStripMenuItem.Visible = False
            End If
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        ToolStripStatusLabel2.Text = Now.Hour & ":" & Now.Minute & ":" & Now.Second
    End Sub

    Private Sub AdministrasiToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AdministrasiToolStripMenuItem.Click
        frmAdministrasi.ShowDialog(Me)
    End Sub

    Private Sub frmMain_Enter(sender As Object, e As EventArgs) Handles MyBase.Enter
        If Not isLogin Then
            MasterToolStripMenuItem.Visible = False
            TransaksiToolStripMenuItem.Visible = False
            LaporanToolStripMenuItem.Visible = False
        Else
            If level = "Kasir" Then
                MasterToolStripMenuItem.Visible = False
            Else
                MasterToolStripMenuItem.Visible = True
            End If
            TransaksiToolStripMenuItem.Visible = True
            LaporanToolStripMenuItem.Visible = True
        End If
    End Sub

    Private Sub frmMain_Activated(sender As Object, e As EventArgs) Handles MyBase.Activated
        If Not isLogin Then
            MasterToolStripMenuItem.Visible = False
            TransaksiToolStripMenuItem.Visible = False
            LaporanToolStripMenuItem.Visible = False
        Else
            If level = "Kasir" Then
                MasterToolStripMenuItem.Visible = False
            Else
                MasterToolStripMenuItem.Visible = True
            End If
            TransaksiToolStripMenuItem.Visible = True
            LaporanToolStripMenuItem.Visible = True
        End If
    End Sub

    Private Sub UserToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UserToolStripMenuItem.Click
        frmuser.ShowDialog(Me)
    End Sub

    Private Sub ApplicationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ApplicationToolStripMenuItem.Click
        frmAboutApp.ShowDialog(Me)
    End Sub

    Private Sub PenjualanUmumToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PenjualanUmumToolStripMenuItem.Click
        frmPenjualanUmum.ShowDialog(Me)
    End Sub

    Private Sub PenjualanAnggotaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PenjualanAnggotaToolStripMenuItem.Click
        frmPenjualanAnggota.ShowDialog(Me)
    End Sub

    Private Sub GajiBulananToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GajiBulananToolStripMenuItem.Click
        frmGajian.ShowDialog(Me)
    End Sub

    Private Sub BayarKreditToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BayarKreditToolStripMenuItem.Click
        frmBayarKredit.ShowDialog(Me)
    End Sub

    Private Sub BarangToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BarangToolStripMenuItem.Click
        frmLapBarang.ShowDialog(Me)
    End Sub

    Private Sub KreditToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles KreditToolStripMenuItem.Click
        frmLapKredit.ShowDialog(Me)
    End Sub

    Private Sub JenisBarangToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles JenisBarangToolStripMenuItem.Click
        frmLapJenisBarang.Show(Me)
    End Sub

    Private Sub PenjualanAnggotaToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles PenjualanAnggotaToolStripMenuItem1.Click
        frmLapPenjualanAnggota.ShowDialog(Me)
    End Sub
End Class
