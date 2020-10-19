#include <cstdint>

struct Vector2D {
    union { float x, r; };
    union { float y, g; };
};

struct Vector3D {
    union { float x, r; };
    union { float y, g; };
    union { float z, b; };
};

struct Vector4D {
    union { float x, r; };
    union { float y, g; };
    union { float z, b; };
    union { float w, a; };
};

struct Matrix4
{
    union
    {
        float elements[4 * 4];
        float matrix[4][4];
        struct
        {
            float a1, a2, a3, a4;
            float b1, b2, b3, b4;
            float c1, c2, c3, c4;
            float d1, d2, d3, d4;
        };
    };
};
