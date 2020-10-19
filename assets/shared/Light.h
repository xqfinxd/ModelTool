#include "BaseStruct.h"

enum LightSourceType : uint32_t {
    eUndefined = 0,
    eDirectional = 1,
    ePoint = 2,
    eSpot = 3,
    eAmbient = 4,
    eArea = 5
};

//pod type
struct Light
{
    LightSourceType LightType{ LightSourceType::eUndefined };
    float AngleInnerCone{ 0.f };
    float AngleOuterCone{ 0.f };
    float AttenuationConstant{ 0.f };
    float AttenuationLinear{ 0.f };
    float AttenuationQuadratic{ 0.f };
    Vector3D Position{ 0.f, 0.f, 0.f };
    Vector3D Direction{ 0.f, 0.f, 0.f };
    Vector3D Up{ 0.f, 0.f, 0.f };
    Vector3D ColorDiffuse{ 0.f, 0.f, 0.f };
    Vector3D ColorSpecular{ 0.f, 0.f, 0.f };
    Vector3D ColorAmbient{ 0.f, 0.f, 0.f };
    Vector2D AreaSize{ 0.f, 0.f };
    char Name[56]{};
    
    Light(void* ptr) {
        memcpy(this, ptr, sizeof(Light));
    }

    Light() = default;
    Light(const Light&) = default;
    Light(Light&&) = default;
    Light& operator=(const Light&) = default;
    Light& operator=(Light&&) = default;
    ~Light() = default;
};

