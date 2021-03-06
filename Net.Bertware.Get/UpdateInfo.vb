﻿Imports System.Xml

Public Class UpdateInfo
    Public Id As UInt16
    Public Pid As UInt16
    Public Version As String
    Public Channel As releasechannel
    Public Forced As Boolean = 0
    Public URL_1 As String
    Public URL_2 As String

    Public ReleaseDate As String
    Public Size As Int32
    Public FileName As String
    Public Sha256 As String = ""
    Public VTPermaLink As String = ""


    Public Sub LoadXML(xml As String)
        Try
            Dim xmldoc As New XmlDocument
            Id = xmldoc.GetElementsByTagName("update")(0).Attributes("id").Value
            Pid = xmldoc.Item("pid").InnerText
            Version = xmldoc.GetElementsByTagName("update")(0).Attributes("version").Value
            Channel = [Enum].Parse(GetType(releasechannel), xmldoc.Item("channel").InnerText)
            Forced = (xmldoc.Item("forced")(0).Value = "1")
            URL_1 = xmldoc.Item("url_1").InnerText
            URL_2 = xmldoc.Item("url_2").InnerText

            ReleaseDate = xmldoc.Item("releasedate").InnerText
            Size = xmldoc.Item("size").InnerText
            FileName = xmldoc.Item("filename").InnerText
            Sha256 = xmldoc.Item("sha256").InnerText
            VTPermaLink = xmldoc.Item("vtlink").InnerText
        Catch ex As Exception
            Trace.WriteLine("Failed to load XML for updateinfo!(text) : " + ex.Message)
        End Try
    End Sub

    Public Sub LoadXML(xmlelement As XmlElement)
        Try

            Id = xmlelement.Attributes("id").InnerText
            Pid = xmlelement.Item("pid").InnerText
            Version = xmlelement.Attributes("version").InnerText
            Channel = [Enum].Parse(GetType(releasechannel), xmlelement.Item("channel").InnerText)
            Forced = (xmlelement.Item("forced").InnerText = "1")
            URL_1 = xmlelement.Item("url_1").InnerText
            URL_2 = xmlelement.Item("url_2").InnerText

            ReleaseDate = xmlelement.Item("releasedate").InnerText
            Size = xmlelement.Item("size").InnerText
            FileName = xmlelement.Item("filename").InnerText
            Sha256 = xmlelement.Item("sha256").InnerText
            VTPermaLink = xmlelement.Item("vtlink").InnerText
        Catch ex As Exception
            Trace.WriteLine("Failed to load XML for updateinfo!(element) : " + ex.Message)
        End Try
    End Sub
End Class
