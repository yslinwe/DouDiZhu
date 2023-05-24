LoginPanel = {}
local this = LoginPanel

this.panelObj = nil

function LoginPanel.Show()
	panelMgr:ShowPanel("LoginPanel", 1)
end

function LoginPanel.Hide()
	panelMgr:HidePanel("LoginPanel")
end

function LoginPanel.OnShow(obj)
	this.panelObj = obj
	local uiBinder = obj:GetComponent('ObjBinder')
	local accountInput = uiBinder:GetObj("accountInputField")
	local passwordInput = uiBinder:GetObj("pwdInputField")
	-- 记住密码勾选
	-- this.rememberTgl = uiBinder:GetObj("rememberToggle"):GetComponent('Toggle')
	this.rememberTgl = uiBinder:SetToggle("rememberToggle", function(v)
		PlayerPrefs.SetString("REMEMBER_TGL", v and "1" or "0")
	end)
	this.rememberTgl.isOn = "1" == PlayerPrefs.GetString("REMEMBER_TGL", "0") 
	-- 条款勾选
	this.clauseTgl = uiBinder:SetToggle("clauseToggle", function(v)
		PlayerPrefs.SetString("CLAUSE_TGL", v and "1" or "0")
	end)
	this.clauseTgl.isOn = "1" == PlayerPrefs.GetString("CLAUSE_TGL", "0") 
	accountInput.text = PlayerPrefs.GetString("LAST_LOGIN_ACCOUNT", "")
	passwordInput.text = PlayerPrefs.GetString("LAST_LOGIN_PWD", "")
	-- 版本号
	-- uiBinder:SetText("versionText", string.format("app: %s res: %s", VersionMgr.instance.appVersion,  VersionMgr.instance.resVersion))
	-- 注册按钮
	uiBinder:SetBtnClick("registButton", function()
		if not this.CheckAccountPwd(accountInput.text, passwordInput.text) then return end
		LoginLogic.DoRegist(accountInput.text, passwordInput.text, function(errorCode, msg)
			if 0 == errorCode then
				this.LoginOk(accountInput.text, passwordInput.text)
			else
				ShowFlyTips.Show(msg)
			end
		end)
	end)
	-- 登录按钮
	uiBinder:SetBtnClick("loginButton", function()
		if not this.CheckAccountPwd(accountInput.text, passwordInput.text) then return end
		LoginLogic.Dologin(accountInput.text, passwordInput.text, function(errorCode, msg)
			if 0 == errorCode then
				this.LoginOk(accountInput.text, passwordInput.text)
			else
				ShowFlyTips.Show(msg)
			end
		end)
	end)
	-- 用户协议
	uiBinder:SetBtnClick("userAgreement", function()
		-- 打开CSDN的条款
		UnityEngine.Application.OpenURL("https://passport.csdn.net/service")
	end)
	-- 隐私条款
	uiBinder:SetBtnClick("privacyPolicy", function()
		-- 打开CSDN的条款
		UnityEngine.Application.OpenURL("https://passport.csdn.net/service")
	end)
end

function LoginPanel.CheckAccountPwd(account, pwd)
	if isStringNilOrEmpty(account) then
		ShowFlyTips.Show("请输入账号")
		return false
	elseif isStringNilOrEmpty(pwd) then
		ShowFlyTips.Show("请输入密码")
		return false
	elseif not this.clauseTgl.isOn then
		ShowFlyTips.Show("请阅读并勾选下方协议")
		return false
	end
	return true
end

function LoginPanel.LoginOk(account, pwd)
	-- 缓存登录的账号
	PlayerPrefs.SetString("LAST_LOGIN_ACCOUNT", account)
	UserData.account = account

	if this.rememberTgl.isOn then
		PlayerPrefs.SetString("LAST_LOGIN_PWD", pwd)
	else
		PlayerPrefs.SetString("LAST_LOGIN_PWD", "")
	end	
	-- 关闭登录界面
	this.Hide()
	-- 显示大厅界面
	PlazaPanel.Show()
end

function LoginPanel.OnHide()
	this.panelObj = nil
end

