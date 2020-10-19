local node = {}

function identity(n)
    local id = {}
    for i = 1, n * n do 
        id[i] = 0
    end
    for i = 1, n do
        id[i * i] = 1
    end
    return id
end

identity4 = identity(4)

function node:instantiate()
    local data = {
        ['parent'] = nil,
        ['children'] = nil,
        ['name'] = '',
        ['mesh'] = {},
        ['light'] = nil,
        ['camera'] = nil,
        ['transform'] = {
            1.0, 0.0, 0.0, 0.0,
            0.0, 1.0, 0.0, 0.0,
            0.0, 0.0, 1.0, 0.0,
            0.0, 0.0, 0.0, 1.0,
        },
        ['metadata'] = {},
    }

    local interface = {
        ['__index'] = self
    }

    return data, interface
end

function node:new()
    local data, interface = self:instantiate()
    setmetatable(data, interface)
    return data
end

function node:addchild()
    if not self.children then
        self.children = {}
    end
    local child = node:new()
    child.parent = self
    table.insert(self.children, child)
    return child
end

function node:getchild(...)
    local indices = {...}
    if #indices < 1 then 
        return nil
    end
    local record = {}
    local child = self
    for _, i in ipairs(indices) do
        if i and type(i) == 'number' and child and 
        child.children and i <= #child.children then
            child = child.children[i]
            table.insert(record, i)
        else
            return nil, record
        end
    end
    return child
end

function node:hasmesh()
    return self.mesh and #self.mesh > 0
end

function node:hascamera()
    return self.camera and true
end

function node:haslight()
    return self.light and true
end

function node:addmesh(name, mesh)
    if mesh then
        table.insert(self.mesh, mesh)
        return true
    end
    return false
end

function node:setcamera(camera)
    if camera then
        self.camera = camera
        return true
    end
    return false
end

function node:setlight(light)
    if light then
        self.light = light
        return true
    end
    return false
end

function node:settransform(transform)
    if transform and #transform == 16 then
        self.transform = transform
        return true
    end
    return false
end

function node:addmetadata(key, value)
    self.metadata[key] = value
end

local scene = {
    ['rootnode'] = node:new()
}

return scene