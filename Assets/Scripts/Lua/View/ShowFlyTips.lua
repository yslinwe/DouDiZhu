ShowFlyTips = {}
local this = ShowFlyTips

function ShowFlyTips.Show(str)
	local uiObj = panelMgr:InstantiateUI(3)
	local uiBinder = uiObj:GetComponent("ObjBinder")
	uiBinder:SetText("TipText", str)
	local aniEvent = uiObj:GetComponent("AnimationEventTrigger")
	aniEvent.aniEvent = function(msg)
		if "finish" == msg then
			safeDestroy(uiObj)
		end
	end
end