﻿using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model;

public sealed class ScreenshotImage : Image
{
    public Screenshot Screenshot { get; private set; }
    
    internal ScreenshotImage(Screenshot screenshot, byte[] content) : base(content)
    {
        Screenshot = screenshot;
    }

    private ScreenshotImage(byte[] content) : base(content)
    {
        Screenshot = null!;
    }
}