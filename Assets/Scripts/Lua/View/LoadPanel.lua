LoadPanel = {}
local this = LoadPanel

this.panelObj = nil

function LoadPanel.Show()
	panelMgr:ShowPanel("LoadPanel", 4)
end

function LoadPanel.Hide()
	panelMgr:HidePanel("LoadPanel")
end

function LoadPanel.OnShow(obj)
	this.panelObj = obj
	local uiBinder = obj:GetComponent('ObjBinder')
	
	local sceneLoader = uiBinder:GetObj("sceneLoader")
	sceneLoader.LoadSceneAsync("main")
	uiBinder:GetObj("versionText")
end

function LoadPanel.OnHide()
	this.panelObj = nil
end