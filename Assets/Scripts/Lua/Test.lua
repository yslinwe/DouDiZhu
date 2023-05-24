require "LuaFileList"
-- require "3rd/pblua/login_pb"
-- require "3rd/pbc/protobuf"

-- local lpeg = require "lpeg"

local json = require "cjson"
local util = require "3rd/cjson/util"

-- local sproto = require "3rd/sproto/sproto"
-- local core = require "sproto.core"
-- local print_r = require "3rd/sproto/print_r"

require "Logic/LuaClass"
function Main()					
	print("logic start")
	LoginPanel.Show() 		
end