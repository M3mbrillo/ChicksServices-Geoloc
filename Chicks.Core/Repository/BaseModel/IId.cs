using System;
using System.Collections.Generic;
using System.Text;

namespace Chicks.Core.Repository.BaseModel
{
    // TODO change to IPK generic
    public interface IId
    {
        public int Id { get; set; }
        bool IsNew() => Id == 0;
    }
}
