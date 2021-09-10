Imports Microsoft.Reporting.WebForms
Imports System.Web.Services.Description
Imports NEA_ICS.UserInterface.NEA_ICS.WcfService
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports System.ServiceModel
Imports System.Reflection

Partial Public Class frmTreeView
    Inherits clsCommonFunction

    Private Shared ModuleList As List(Of RoleDetails)

#Region " Form Variables "
    Dim bnlExpand_F As Boolean
    Dim bnlExpand_R As Boolean
    Dim bnlExpand_C As Boolean
#End Region

#Region " Page Load "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            PopulateTreeView()
        End If
    End Sub
#End Region

#Region " Function - Populate Tree View "
    Private Sub PopulateTreeView()

        Try

            Dim parentNode As TreeNode = Nothing

            Dim Client As New ServiceClient
            Dim RoleDetails As New RoleDetails

            RoleDetails.StoreID = Session("StoreID")
            RoleDetails.UserID = Session("UserID")

            ModuleList = Client.GetModuleAccess(RoleDetails)
            Client.Close()

            tvICS.Nodes.Clear()

            For Each ModuleItem As RoleDetails In ModuleList

                If ModuleItem.ParentID = 0 Then

                    parentNode = New TreeNode
                    parentNode.Text = " " & ModuleItem.ModuleTitle
                    parentNode.Value = ModuleItem.ModuleID

                    tvICS.Nodes.Add(parentNode)

                Else

                    Dim childNode As New TreeNode

                    childNode.Text = "&nbsp;" & ModuleItem.ModuleTitle
                    childNode.Value = ModuleItem.ModuleID

                    Select Case ModuleItem.ModuleType
                        Case "F"
                            childNode.ImageUrl = "Images/fld.gif"
                        Case "N"
                            childNode.ImageUrl = "Images/fld_exp.gif"
                    End Select

                    If ModuleItem.ModuleSource <> "#" Then
                        childNode.NavigateUrl = ModuleItem.ModuleSource
                    End If

                    parentNode.ChildNodes.Add(childNode)

                End If

            Next

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "UserInterface Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

    End Sub
#End Region

End Class