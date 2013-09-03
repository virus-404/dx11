﻿

using System.Collections.Generic;
using System.Runtime.InteropServices;
using SlimDX;
using SlimDX.DXGI;
using SlimDX.Direct3D11;
using Device = SlimDX.Direct3D11.Device;

namespace Core.Model {
    public class MeshGeometry : DisposableClass {
        public class Subset {
            public int Id;
            public int VertexStart;
            public int VertexCount;
            public int FaceStart;
            public int FaceCount;
            public Subset() {
                Id = -1;
            }
        }

        private Buffer _vb;
        private Buffer _ib;
        private Format _indexBufferFormat;
        private int _vertexStride;
        private List<Subset> _subsetTable;
        private bool _disposed;

        public MeshGeometry() {
            _indexBufferFormat = Format.R16_UInt;
            _vertexStride = 0;
        }
        protected override void Dispose(bool disposing) {
            if (!_disposed) {
                if (disposing) {
                    Util.ReleaseCom(ref _vb);
                    Util.ReleaseCom(ref _ib);
                }
                _disposed = true;
            }
            base.Dispose(disposing);
        }
        public void SetVertices<TVertexType>(Device device, List<TVertexType> vertices) where TVertexType : struct {
            Util.ReleaseCom(ref _vb);
            _vertexStride = Marshal.SizeOf(typeof (TVertexType));

            var vbd = new BufferDescription(_vertexStride*vertices.Count, ResourceUsage.Immutable, BindFlags.VertexBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
            _vb = new Buffer(device, new DataStream(vertices.ToArray(), false, false), vbd);
        }
        public void SetIndices(Device device, List<short> indices) {
            var ibd = new BufferDescription(sizeof (short)*indices.Count, ResourceUsage.Immutable, BindFlags.IndexBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
            _ib = new Buffer(device, new DataStream(indices.ToArray(), false, false), ibd);
        }
        public void SetSubsetTable(List<Subset> subsets) {
            _subsetTable = subsets;
        }
        public void Draw(DeviceContext dc, int subsetId) {
            const int offset = 0;
            dc.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(_vb, _vertexStride, offset));
            dc.InputAssembler.SetIndexBuffer(_ib, Format.R16_UInt, 0);
            dc.DrawIndexed(_subsetTable[subsetId].FaceCount*3, _subsetTable[subsetId].FaceStart*3, 0);
        }
    }
}
