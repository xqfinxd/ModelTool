#include "BaseStruct.h"

//pod type
struct Camera
{
    Vector3D Position{ 0.f, 0.f, 0.f };
    Vector3D Up{ 0.f, 0.f, 0.f };
    Vector3D Direction{ 0.f, 0.f, 0.f };
    float FieldOfview{ 0.f };
    float ClipPlaneNear{ 0.f };
    float ClipPlaneFar{ 0.f };
    float AspectRatio{ 0.f };
    Matrix4 ViewMatrix{};
    char Name[44]{};

    Camera(void* ptr) {
        memcpy(this, ptr, sizeof(Camera));
    }

    Camera() = default;
    Camera(const Camera&) = default;
    Camera(Camera&&) = default;
    Camera& operator=(const Camera&) = default;
    Camera& operator=(Camera&&) = default;
    ~Camera() = default;
};