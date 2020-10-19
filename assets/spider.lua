
local scene = dofile('E:/GITHUB/ModelTool/assets/shared/model.lua')

local root = scene.rootnode
local name = 'spider'
local node = scene.rootnode

node.name = "RootNode"
node:settransform({
    1, 0, 0, 0,
    0, 1, 0, 0,
    0, 0, 1, 0,
    0, 0, 0, 1,
})
node = node:addchild()
node.name = "klZahn"
node:settransform({
    100, 0, 0, 0,
    0, 100, 0, 0,
    0, 0, 100, 0,
    0, 0, 0, 1,
})
node:addmesh("klZahn","E:/GITHUB/ModelTool/assets/spider/0000-klZahn")
node = node.parent

node = node:addchild()
node.name = "OK"
node:settransform({
    100, 0, 0, 0,
    0, 100, 0, 0,
    0, 0, 100, 0,
    0, 0, 0, 1,
})
node:addmesh("OK","E:/GITHUB/ModelTool/assets/spider/0001-OK")
node = node.parent

node = node:addchild()
node.name = "Bein3Li"
node:settransform({
    100, 0, 0, 0,
    0, 100, 0, 0,
    0, 0, 100, 0,
    0, 0, 0, 1,
})
node:addmesh("Bein3Li","E:/GITHUB/ModelTool/assets/spider/0002-Bein3Li")
node = node.parent

node = node:addchild()
node.name = "Auge"
node:settransform({
    100, 0, 0, 0,
    0, 100, 0, 0,
    0, 0, 100, 0,
    0, 0, 0, 1,
})
node:addmesh("Auge","E:/GITHUB/ModelTool/assets/spider/0003-Auge")
node = node.parent

node = node:addchild()
node.name = "Kopf2"
node:settransform({
    100, 0, 0, 0,
    0, 100, 0, 0,
    0, 0, 100, 0,
    0, 0, 0, 1,
})
node:addmesh("Kopf2","E:/GITHUB/ModelTool/assets/spider/0004-Kopf2")
node = node.parent

node = node:addchild()
node.name = "Zahn"
node:settransform({
    100, 0, 0, 0,
    0, 100, 0, 0,
    0, 0, 100, 0,
    0, 0, 0, 1,
})
node:addmesh("Zahn","E:/GITHUB/ModelTool/assets/spider/0005-Zahn")
node = node.parent

node = node:addchild()
node.name = "Bein3Re"
node:settransform({
    100, 0, 0, 0,
    0, 100, 0, 0,
    0, 0, 100, 0,
    0, 0, 0, 1,
})
node:addmesh("Bein3Re","E:/GITHUB/ModelTool/assets/spider/0006-Bein3Re")
node = node.parent

node = node:addchild()
node.name = "Bein4Re"
node:settransform({
    100, 0, 0, 0,
    0, 100, 0, 0,
    0, 0, 100, 0,
    0, 0, 0, 1,
})
node:addmesh("Bein4Re","E:/GITHUB/ModelTool/assets/spider/0007-Bein4Re")
node = node.parent

node = node:addchild()
node.name = "Bein4Li"
node:settransform({
    100, 0, 0, 0,
    0, 100, 0, 0,
    0, 0, 100, 0,
    0, 0, 0, 1,
})
node:addmesh("Bein4Li","E:/GITHUB/ModelTool/assets/spider/0008-Bein4Li")
node = node.parent

node = node:addchild()
node.name = "Brust"
node:settransform({
    100, 0, 0, 0,
    0, 100, 0, 0,
    0, 0, 100, 0,
    0, 0, 0, 1,
})
node:addmesh("Brust","E:/GITHUB/ModelTool/assets/spider/0009-Brust")
node = node.parent

node = node:addchild()
node.name = "klZahn2"
node:settransform({
    100, 0, 0, 0,
    0, 100, 0, 0,
    0, 0, 100, 0,
    0, 0, 0, 1,
})
node:addmesh("klZahn2","E:/GITHUB/ModelTool/assets/spider/0010-klZahn2")
node = node.parent

node = node:addchild()
node.name = "Bein2Li"
node:settransform({
    100, 0, 0, 0,
    0, 100, 0, 0,
    0, 0, 100, 0,
    0, 0, 0, 1,
})
node:addmesh("Bein2Li","E:/GITHUB/ModelTool/assets/spider/0011-Bein2Li")
node = node.parent

node = node:addchild()
node.name = "HLeib01"
node:settransform({
    100, 0, 0, 0,
    0, 100, 0, 0,
    0, 0, 100, 0,
    0, 0, 0, 1,
})
node:addmesh("HLeib01","E:/GITHUB/ModelTool/assets/spider/0012-HLeib01")
node = node.parent

node = node:addchild()
node.name = "Kopf"
node:settransform({
    100, 0, 0, 0,
    0, 100, 0, 0,
    0, 0, 100, 0,
    0, 0, 0, 1,
})
node:addmesh("Kopf","E:/GITHUB/ModelTool/assets/spider/0013-Kopf")
node = node.parent

node = node:addchild()
node.name = "Zahn2"
node:settransform({
    100, 0, 0, 0,
    0, 100, 0, 0,
    0, 0, 100, 0,
    0, 0, 0, 1,
})
node:addmesh("Zahn2","E:/GITHUB/ModelTool/assets/spider/0014-Zahn2")
node = node.parent

node = node:addchild()
node.name = "Bein1Re"
node:settransform({
    100, 0, 0, 0,
    0, 100, 0, 0,
    0, 0, 100, 0,
    0, 0, 0, 1,
})
node:addmesh("Bein1Re","E:/GITHUB/ModelTool/assets/spider/0015-Bein1Re")
node = node.parent

node = node:addchild()
node.name = "Bein1Li"
node:settransform({
    100, 0, 0, 0,
    0, 100, 0, 0,
    0, 0, 100, 0,
    0, 0, 0, 1,
})
node:addmesh("Bein1Li","E:/GITHUB/ModelTool/assets/spider/0016-Bein1Li")
node = node.parent

node = node:addchild()
node.name = "Bein2Re"
node:settransform({
    100, 0, 0, 0,
    0, 100, 0, 0,
    0, 0, 100, 0,
    0, 0, 0, 1,
})
node:addmesh("Bein2Re","E:/GITHUB/ModelTool/assets/spider/0017-Bein2Re")
node = node.parent

node = node:addchild()
node.name = "Duplicate05"
node:settransform({
    100, 0, 0, 0,
    0, 100, 0, 0,
    0, 0, 100, 0,
    0, 0, 0, 1,
})
node:addmesh("Duplicate05","E:/GITHUB/ModelTool/assets/spider/0018-Duplicate05")
node = node.parent

node = node:addchild()
node.name = "Lamp"
node:settransform({
    -29.08647, 56.63931, 77.1101, 407.6245,
    -5.518904, 79.46724, -60.45247, 590.3862,
    -95.51712, -21.83912, -19.98834, -100.5454,
    0, 0, 0, 1,
})
node:setlight("E:/GITHUB/ModelTool/assets/spider/0000-Lamp.light")
node = node.parent

node = node:addchild()
node.name = "Camera"
node:settransform({
    -65.48616, -31.73701, 68.58808, 748.1132,
    -44.52453, 89.53433, -1.08166, 534.3665,
    -61.06659, -31.24686, -72.76335, 650.764,
    0, 0, 0, 1,
})
node:setcamera("E:/GITHUB/ModelTool/assets/spider/0000-Camera.camera")
node = node.parent

node = node.parent

