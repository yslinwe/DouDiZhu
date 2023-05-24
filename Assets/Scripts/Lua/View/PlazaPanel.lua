PlazaPanel = {}
local this = PlazaPanel

this.panelObj = nil

function PlazaPanel.Show()
	panelMgr:ShowPanel("PlazaPanel", 2)
end

function PlazaPanel.Hide()
	panelMgr:HidePanel("PlazaPanel")
end

function PlazaPanel.OnShow(obj)
	this.panelObj = obj
	local uiBinder = obj:GetComponent('ObjBinder')
	uiBinder:GetObj("CoinText")
	uiBinder:GetObj("DiamondText")
	uiBinder:SetBtnClick("easyBtn", function() 
		this.Hide()
		LoadPanel.difficultyType = "easy"
		LoadPanel.Show()
	end)
	uiBinder:SetBtnClick("hardBtn", function() 
		this.Hide()
		LoadPanel.difficultyType = "hard"
		LoadPanel.Show()
	end)
	-- 账号名
	uiBinder:SetText("AccountNameText", UserData.account)
	-- 返回按钮
	uiBinder:SetBtnClick("outButton", function() 
		this.Hide()
		LoginPanel.Show()
	end)
end

function PlazaPanel.OnHide()
	this.panelObj = nil
end