﻿Imports System.Threading
Imports System.Threading.Thread

Partial Class Main

    Private Sub Download()
        If Me.Enable(1) = True Then Call JMA_Weather_Chart()
        If Me.Enable(2) = True Then Call CWB_Weather_Chart()
        If Me.Enable(3) = True Then Call CWB_Skew_T()
        If Me.Enable(4) = True Then Call CWB_Radar()
        If Me.Enable(5) = True Then Call CWB_Satellite()
        If Me.Enable(6) = True Then Call CWB_Temp()
        If Me.Enable(7) = True Then Call CWB_Rain()
        If Me.Enable(8) = True Then Call CWB_850hpa_WindSpeed_Streamline()
        If Me.Enable(9) = True Then Call CWB_850hpa_RH_Streamline()
        If Me.Enable(10) = True Then Call JMA_WaterVapor_Image()

        Sleep(2000)
        If DisplayUI = True Then
            Me.Invoke(New MethodInvoker(AddressOf Done))
        Else
            Call Done()
        End If
    End Sub

    Private Sub JMA_Weather_Chart()
        '日本氣象廳天氣圖_D1
        'http://www.hbc.co.jp/tecweather/archive/pdf/ASAS_042509.pdf  アジア地上解析天気図(ASAS) (every 6 hr)
        'http://www.hbc.co.jp/tecweather/archive/pdf/AUPQ78_042509.pdf アジア850hPa・700hPa天気図(AUPQ78) (every 12 hr)
        'http://www.hbc.co.jp/tecweather/archive/pdf/AUPQ35_042509.pdf アジア500hPa・300hPa天気図(AUPQ35) (every 12 hr)
        Dim Checkhours As String
        Me.D = 1
        Me.DS(D) = 1
        Call PreRefreshState()
        If Dir(Application.StartupPath + "\日本氣象廳天氣圖", vbDirectory) = "" Then MkDir(Application.StartupPath + "\日本氣象廳天氣圖")

        ''lastest data part
        DownloadFile(True, "http://www.jma.go.jp/jp/metcht/pdf/kosou/aupq35_00.pdf", Application.StartupPath + "\日本氣象廳天氣圖", "aupq35_00_download_at_" + DateString + "_" + Time.Substring(0, 2) + "-" + Time.Substring(2, 2) + ".pdf")
        DownloadFile(True, "http://www.jma.go.jp/jp/metcht/pdf/kosou/aupq35_12.pdf", Application.StartupPath + "\日本氣象廳天氣圖", "aupq35_12_download_at_" + DateString + "_" + Time.Substring(0, 2) + "-" + Time.Substring(2, 2) + ".pdf")
        DownloadFile(True, "http://www.jma.go.jp/jp/metcht/pdf/kosou/aupq78_00.pdf", Application.StartupPath + "\日本氣象廳天氣圖", "aupq78_00_download_at_" + DateString + "_" + Time.Substring(0, 2) + "-" + Time.Substring(2, 2) + ".pdf")
        DownloadFile(True, "http://www.jma.go.jp/jp/metcht/pdf/kosou/aupq78_12.pdf", Application.StartupPath + "\日本氣象廳天氣圖", "aupq78_12_download_at_" + DateString + "_" + Time.Substring(0, 2) + "-" + Time.Substring(2, 2) + ".pdf")

        ''historyical data part
        'today - prevent download last year data
        TimeForName = Format(Now.AddDays(+0), "MMdd") ''0708 = 7/8
        Checkhours = Format(Now.AddHours(-5), "MMdd")  ''japan time = 03
        If Checkhours = TimeForName Then
            DownloadFile(False, "http://www.hbc.jp/tecweather/archive/pdf/ASAS_" + TimeForName + "03.pdf", Application.StartupPath + "\日本氣象廳天氣圖", "ASAS_" + TimeForName + "03.pdf")
        End If
        Checkhours = Format(Now.AddHours(-11), "MMdd")  ''japan time = 09
        If Checkhours = TimeForName Then
            DownloadFile(False, "http://www.hbc.jp/tecweather/archive/pdf/ASAS_" + TimeForName + "09.pdf", Application.StartupPath + "\日本氣象廳天氣圖", "ASAS_" + TimeForName + "09.pdf")
            DownloadFile(False, "http://www.hbc.jp/tecweather/archive/pdf/AUPQ78_" + TimeForName + "09.pdf", Application.StartupPath + "\日本氣象廳天氣圖", "AUPQ78_" + TimeForName + "09.pdf")
            DownloadFile(False, "http://www.hbc.jp/tecweather/archive/pdf/AUPQ35_" + TimeForName + "09.pdf", Application.StartupPath + "\日本氣象廳天氣圖", "AUPQ35_" + TimeForName + "09.pdf")
        End If
        Checkhours = Format(Now.AddHours(-17), "MMdd")  ''japan time = 15
        If Checkhours = TimeForName Then
            DownloadFile(False, "http://www.hbc.jp/tecweather/archive/pdf/ASAS_" + TimeForName + "15.pdf", Application.StartupPath + "\日本氣象廳天氣圖", "ASAS_" + TimeForName + "15.pdf")
        End If
        Checkhours = Format(Now.AddHours(-23), "MMdd")  ''japan time = 21
        If Checkhours = TimeForName Then
            DownloadFile(False, "http://www.hbc.jp/tecweather/archive/pdf/ASAS_" + TimeForName + "21.pdf", Application.StartupPath + "\日本氣象廳天氣圖", "ASAS_" + TimeForName + "21.pdf")
            'DownloadFile(False, "http://www.hbc.jp/tecweather/archive/pdf/AUPQ78_" + TimeForName + "21.pdf", Application.StartupPath + "\日本氣象廳天氣圖", "AUPQ78_" + TimeForName + "21.pdf")
            'DownloadFile(False, "http://www.hbc.jp/tecweather/archive/pdf/AUPQ35_" + TimeForName + "21.pdf", Application.StartupPath + "\日本氣象廳天氣圖", "AUPQ35_" + TimeForName + "21.pdf")
        End If
        'before
        For i = 1 To Me.Period(D) Step 1
            TimeForName = Format(Now.AddDays(-1 * i), "yyyyMMddHHmm") ''201407081107
            DownloadFile(False, "http://www.hbc.jp/tecweather/archive/pdf/ASAS_" + TimeForName.Substring(4, 4) + "21.pdf", Application.StartupPath + "\日本氣象廳天氣圖", "ASAS_" + TimeForName.Substring(4, 4) + "21.pdf")
            DownloadFile(False, "http://www.hbc.jp/tecweather/archive/pdf/ASAS_" + TimeForName.Substring(4, 4) + "15.pdf", Application.StartupPath + "\日本氣象廳天氣圖", "ASAS_" + TimeForName.Substring(4, 4) + "15.pdf")
            DownloadFile(False, "http://www.hbc.jp/tecweather/archive/pdf/ASAS_" + TimeForName.Substring(4, 4) + "09.pdf", Application.StartupPath + "\日本氣象廳天氣圖", "ASAS_" + TimeForName.Substring(4, 4) + "09.pdf")
            DownloadFile(False, "http://www.hbc.jp/tecweather/archive/pdf/ASAS_" + TimeForName.Substring(4, 4) + "03.pdf", Application.StartupPath + "\日本氣象廳天氣圖", "ASAS_" + TimeForName.Substring(4, 4) + "03.pdf")
            DownloadFile(False, "http://www.hbc.jp/tecweather/archive/pdf/AUPQ78_" + TimeForName.Substring(4, 4) + "21.pdf", Application.StartupPath + "\日本氣象廳天氣圖", "AUPQ78_" + TimeForName.Substring(4, 4) + "21.pdf")
            DownloadFile(False, "http://www.hbc.jp/tecweather/archive/pdf/AUPQ78_" + TimeForName.Substring(4, 4) + "09.pdf", Application.StartupPath + "\日本氣象廳天氣圖", "AUPQ78_" + TimeForName.Substring(4, 4) + "09.pdf")
            DownloadFile(False, "http://www.hbc.jp/tecweather/archive/pdf/AUPQ35_" + TimeForName.Substring(4, 4) + "21.pdf", Application.StartupPath + "\日本氣象廳天氣圖", "AUPQ35_" + TimeForName.Substring(4, 4) + "21.pdf")
            DownloadFile(False, "http://www.hbc.jp/tecweather/archive/pdf/AUPQ35_" + TimeForName.Substring(4, 4) + "09.pdf", Application.StartupPath + "\日本氣象廳天氣圖", "AUPQ35_" + TimeForName.Substring(4, 4) + "09.pdf")
        Next
        Me.DS(D) = 2
        Call RepeatFileRemover.RepeatFileRemoverMain(Application.StartupPath + "\日本氣象廳天氣圖\")
        Call PreRefreshState()
    End Sub

    Private Sub CWB_Weather_Chart()
        'CWB地面天氣圖_D2
        'http://www.cwb.gov.tw/V7/forecast/fcst/Data/2014-0508-0600_SFCcombo.jpg (every 6 hr)
        Me.D = 2
        Me.DS(D) = 1
        Call PreRefreshState()
        If Dir(Application.StartupPath + "\CWB地面天氣圖", vbDirectory) = "" Then MkDir(Application.StartupPath + "\CWB地面天氣圖")
        For i = 0 To Me.Period(D) Step 1
            TimeForName = Format(Now.AddDays(-1 * i), "yyyy-MMdd") ''2014-0708
            DownloadFile(True, "http://www.cwb.gov.tw/V7/forecast/fcst/Data/" + TimeForName + "-0600_SFCcombo.jpg", Application.StartupPath + "\CWB地面天氣圖", TimeForName + "-0600_SFCcombo.jpg")
            DownloadFile(True, "http://www.cwb.gov.tw/V7/forecast/fcst/Data/" + TimeForName + "-1200_SFCcombo.jpg", Application.StartupPath + "\CWB地面天氣圖", TimeForName + "-1200_SFCcombo.jpg")
            DownloadFile(True, "http://www.cwb.gov.tw/V7/forecast/fcst/Data/" + TimeForName + "-1800_SFCcombo.jpg", Application.StartupPath + "\CWB地面天氣圖", TimeForName + "-1800_SFCcombo.jpg")
            DownloadFile(True, "http://www.cwb.gov.tw/V7/forecast/fcst/Data/" + TimeForName + "-0000_SFCcombo.jpg", Application.StartupPath + "\CWB地面天氣圖", TimeForName + "-0000_SFCcombo.jpg")
        Next
        Me.DS(D) = 2
        Call PreRefreshState()
    End Sub

    Private Sub CWB_Skew_T()
        '斜溫圖_D3
        Me.D = 3
        Me.DS(D) = 1
        Call PreRefreshState()
        If Dir(Application.StartupPath + "\斜溫圖", vbDirectory) = "" Then MkDir(Application.StartupPath + "\斜溫圖")
        DownloadFile(True, "http://www.cwb.gov.tw/V7/station/Data/SKW_46692.pdf", Application.StartupPath + "\斜溫圖", "SKW_46692_" + DateString + "_" + Time.Substring(0, 2) + "-" + Time.Substring(2, 2) + ".pdf")
        DownloadFile(True, "http://www.cwb.gov.tw/V7/station/Data/SKW_46699.pdf", Application.StartupPath + "\斜溫圖", "SKW_46699_" + DateString + "_" + Time.Substring(0, 2) + "-" + Time.Substring(2, 2) + ".pdf")
        DownloadFile(True, "http://www.cwb.gov.tw/V7/station/Data/SKW_46750.pdf", Application.StartupPath + "\斜溫圖", "SKW_46750_" + DateString + "_" + Time.Substring(0, 2) + "-" + Time.Substring(2, 2) + ".pdf")
        Me.DS(3) = 2
        Call RepeatFileRemover.RepeatFileRemoverMain(Application.StartupPath + "\斜溫圖\")
        Call PreRefreshState()
    End Sub

    Private Sub CWB_Radar()
        '雷達回波圖_D4
        'http://www.cwb.gov.tw/V7/observe/radar/Data/HD_Radar/CV1_3600_201605161950.jpg
        Dim Min As String
        Me.D = 4
        Me.DS(D) = 1
        Call PreRefreshState()
        If Dir(Application.StartupPath + "\雷達回波圖", vbDirectory) = "" Then MkDir(Application.StartupPath + "\雷達回波圖")
        For i = 0 To Me.Period(D)
            TimeForName = Format(Now.AddHours(-1 * i), "yyyyMMddHH")
            For j = 0 To 50 Step Me.Radar_density
                Min = j.ToString
                If Min.Length = 1 Then Min = "0" + Min
                DownloadFile(True, "http://www.cwb.gov.tw/V7/observe/radar/Data/HD_Radar/CV1_3600_" + TimeForName + Min + ".jpg", Application.StartupPath + "\雷達回波圖", TimeForName + Min + ".2MOS0.jpg")
            Next
        Next
        Me.DS(D) = 2
        Call PreRefreshState()
    End Sub

    Private Sub CWB_Satellite()
        '衛星雲圖_D5 
        'http://www.cwb.gov.tw/V7/observe/satellite/Data/HSAO/HSAO-2014-04-20-11-30.jpg 
        'http://www.cwb.gov.tw/V7/observe/satellite/Data/HS1Q/HS1Q-2014-04-20-11-30.jpg
        Me.D = 5
        Me.DS(D) = 1
        Call PreRefreshState()
        If Dir(Application.StartupPath + "\衛星雲圖", vbDirectory) = "" Then MkDir(Application.StartupPath + "\衛星雲圖")
        For i = 0 To Me.Period(D)
            TimeForName = Format(Now.AddHours(-1 * i), "yyyy-MM-dd-HH") ''2014-07-08-23
            '整點
            DownloadFile(True, "http://www.cwb.gov.tw/V7/observe/satellite/Data/HSAO/HSAO-" + TimeForName + "-00.jpg", Application.StartupPath + "\衛星雲圖", "HSAO-" + TimeForName + "-00.jpg") '可見光
            DownloadFile(True, "http://www.cwb.gov.tw/V7/observe/satellite/Data/HS1Q/HS1Q-" + TimeForName + "-00.jpg", Application.StartupPath + "\衛星雲圖", "HS1Q-" + TimeForName + "-00.jpg") '色調強化
            '半點
            DownloadFile(True, "http://www.cwb.gov.tw/V7/observe/satellite/Data/HSAO/HSAO-" + TimeForName + "-30.jpg", Application.StartupPath + "\衛星雲圖", "HSAO-" + TimeForName + "-30.jpg") '可見光
            DownloadFile(True, "http://www.cwb.gov.tw/V7/observe/satellite/Data/HS1Q/HS1Q-" + TimeForName + "-30.jpg", Application.StartupPath + "\衛星雲圖", "HS1Q-" + TimeForName + "-30.jpg") '色調強化
        Next
        Me.DS(D) = 2
        Call PreRefreshState()
    End Sub


    Private Sub CWB_Temp()
        '溫度分布圖_D6 
        'http://www.cwb.gov.tw/V7/observe/temperature/Data/2014-04-20_2000.GTP.jpg
        Me.D = 6
        Me.DS(D) = 1
        Call PreRefreshState()
        If Dir(Application.StartupPath + "\溫度分布圖 ", vbDirectory) = "" Then MkDir(Application.StartupPath + "\溫度分布圖 ")
        For i = 0 To Me.Period(D)
            TimeForName = Format(Now.AddHours(-1 * i), "yyyy-MM-dd_HH") ''2014-07-08_23
            DownloadFile(True, "http://www.cwb.gov.tw/V7/observe/temperature/Data/" + TimeForName + "00.GTP.jpg", Application.StartupPath + "\溫度分布圖", TimeForName + "00.GTP.jpg")
        Next
        Me.DS(D) = 2
        Call PreRefreshState()
    End Sub

    Private Sub CWB_Rain()
        '日累積雨量圖_D7
        'http://www.cwb.gov.tw/V7/observe/rainfall/Data/hka09100.jpg
        Dim MonthCheck As Integer
        Me.D = 7
        Me.DS(D) = 1
        Call PreRefreshState()
        If Dir(Application.StartupPath + "\日累積雨量圖", vbDirectory) = "" Then MkDir(Application.StartupPath + "\日累積雨量圖")
        For i = 0 To Me.Period(D)
            MonthCheck = Format(Now.AddHours(-1 * i), "MM")
            If MonthCheck = 10 Then
                TimeForName = Format(Now.AddHours(-1 * i), "ddHH")
                TimeForName = "a" + TimeForName
            ElseIf MonthCheck = 11 Then
                TimeForName = Format(Now.AddHours(-1 * i), "ddHH")
                TimeForName = "b" + TimeForName
            ElseIf MonthCheck = 12 Then
                TimeForName = Format(Now.AddHours(-1 * i), "ddHH")
                TimeForName = "c" + TimeForName
            ElseIf MonthCheck < 10 Then
                TimeForName = Format(Now.AddHours(-1 * i), "MddHH") ''0823
            End If
            DownloadFile(True, "http://www.cwb.gov.tw/V7/observe/rainfall/Data/hk" + TimeForName + "0.jpg", Application.StartupPath + "\日累積雨量圖", "hk" + TimeForName + "-00.jpg") '可見光
            DownloadFile(True, "http://www.cwb.gov.tw/V7/observe/rainfall/Data/hk" + TimeForName + "3.jpg", Application.StartupPath + "\日累積雨量圖", "hk" + TimeForName + "-30.jpg") '可見光
        Next
        Me.DS(D) = 2
        Call PreRefreshState()
    End Sub

    Private Sub CWB_850hpa_WindSpeed_Streamline()
        '850hpa風速+流線圖_D8
        'http://www.cwb.gov.tw/V7/forecast/nwp/Data/GFS/GFS_14041918_DS2-GE_000.gif
        Dim Hour As Integer
        Me.D = 8
        Me.DS(D) = 1
        Call PreRefreshState()
        If Dir(Application.StartupPath + "\850hpa風速+流線圖 ", vbDirectory) = "" Then MkDir(Application.StartupPath + "\850hpa風速+流線圖 ")
        For i = 0 To Me.Period(D)
            TimeForName = Format(Now.AddHours(-1 * i), "yyyyMMddHH") ''2014070823
            Hour = Convert.ToInt32(TimeForName.Substring(8, 2))
            If ((Hour + 24) Mod 6) = 0 Then
                DownloadFile(True, "http://www.cwb.gov.tw/V7/forecast/nwp/Data/GFS/GFS_" + TimeForName.Substring(2, 8) + "_DS2-GE_000.gif", Application.StartupPath + "\850hpa風速+流線圖", "GFS_" + TimeForName.Substring(2, 8) + "_DS2-GE_000.gif")
            End If
        Next
        Me.DS(D) = 2
        Call PreRefreshState()
    End Sub

    Private Sub CWB_850hpa_RH_Streamline()
        '850hpa相對溼度+流線圖_D9
        'http://www.cwb.gov.tw/V7/forecast/nwp/Data/GFS/GFS_15020200_D51D2S-GE_000.gif
        Dim Hour As Integer
        Me.D = 9
        Me.DS(D) = 1
        Call PreRefreshState()
        If Dir(Application.StartupPath + "\850hpa相對溼度+流線圖 ", vbDirectory) = "" Then MkDir(Application.StartupPath + "\850hpa相對溼度+流線圖 ")
        For i = 0 To Me.Period(D)
            TimeForName = Format(Now.AddHours(-1 * i), "yyyyMMddHH") ''2014070823
            Hour = Convert.ToInt32(TimeForName.Substring(8, 2))
            If ((Hour + 24) Mod 6) = 0 Then
                DownloadFile(True, "http://www.cwb.gov.tw/V7/forecast/nwp/Data/GFS/GFS_" + TimeForName.Substring(2, 8) + "_D51D2S-GE_000.gif", Application.StartupPath + "\850hpa相對溼度+流線圖", "GFS_" + TimeForName.Substring(2, 8) + "_DS2-GE_000.gif")
            End If
        Next
        Me.DS(D) = 2
        Call PreRefreshState()
    End Sub

    Private Sub JMA_WaterVapor_Image()
        '日本氣象廳衛星雲圖(水氣頻道)_D10
        'http://www.jma.go.jp/jp/gms/imgs/1/watervapor/1/201502021715-00.png
        Me.D = 10
        Me.DS(D) = 1
        Call PreRefreshState()
        If Dir(Application.StartupPath + "\日本氣象廳衛星雲圖(水氣頻道) ", vbDirectory) = "" Then MkDir(Application.StartupPath + "\日本氣象廳衛星雲圖(水氣頻道)")
        For i = 0 To Me.Period(D)
            TimeForName = Format(Now.AddHours(-1 * i), "yyyyMMddHH") ''ex:2014070823
            '00
            DownloadFile(True, "http://www.jma.go.jp/jp/gms/imgs/1/watervapor/1/" + TimeForName + "00-00.png", Application.StartupPath + "\日本氣象廳衛星雲圖(水氣頻道)", TimeForName + "00-00.png")
            '15
            DownloadFile(True, "http://www.jma.go.jp/jp/gms/imgs/1/watervapor/1/" + TimeForName + "15-00.png", Application.StartupPath + "\日本氣象廳衛星雲圖(水氣頻道)", TimeForName + "15-00.png")
            '30
            'DownloadFile(True, "http://www.jma.go.jp/jp/gms/imgs/1/watervapor/1/" + TimeForName + "30-00.png", Application.StartupPath + "\日本氣象廳衛星雲圖(水氣頻道)", TimeForName + "30-00.png")
            '45
            'DownloadFile(True, "http://www.jma.go.jp/jp/gms/imgs/1/watervapor/1/" + TimeForName + "45-00.png", Application.StartupPath + "\日本氣象廳衛星雲圖(水氣頻道)", TimeForName + "45-00.png")
        Next
        Me.DS(D) = 2
        Call PreRefreshState()
    End Sub

    '用WebClient來下載
    '要使用的話呼叫DownloadFile並傳入3個參數,1是要下載的檔案的url,2是要存放在那邊,3是要存放的檔名,下載完成會傳回檔案的存放路徑,失敗的話 則回傳空字串
    'ex : DownloadFile("http://www.morpheus2005.altervista.org/xp%20Gallery/Windows%20XP%20Logo%20-%20Coloured%20Glass.jpg", "C:\", "aa.jpg") 
    Public Sub DownloadFile(ByVal check As Boolean, ByVal strUrl As String, ByVal strFile As String, ByVal strFileName As String)
        If check = True Then
            If Not Dir(strFile + "\" + strFileName, vbArchive) = "" Then Exit Sub
        End If
        Try
            Dim strFilePath As String = strFile + "\" + strFileName
            Dim dwl As New System.Net.WebClient()
            dwl.DownloadFile(strUrl, strFilePath)
            dwl.Dispose()
            DN(D) = DN(D) + 1
        Catch ex As Exception
            'MessageBox.Show("錯誤:" + ex.ToString, "錯誤通知", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            If ErrorLog(D) = True Then
                RecordNum = FreeFile()
                FileOpen(RecordNum, Application.StartupPath + "\Record.txt", OpenMode.Append)
                PrintLine(RecordNum, DateString + "_" + TimeString + "_" + Time + "_" + "Error! file=" + strUrl + "   " + ex.ToString)
                FileClose(RecordNum)
            End If
        End Try
    End Sub
End Class
