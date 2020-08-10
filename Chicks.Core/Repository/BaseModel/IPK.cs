using MongoDB.Bson;
using MongoDB.Bson.IO;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;

namespace Chicks.Core.Repository.BaseModel
{
    public interface IPK
    {
        //Expression<Func<TImplement, bool>> ExpressionMatchPk<TImplement>();
        //bool MatchPk<TImplement>(TImplement x);

        bool IsNew();
    }

    public interface IId : IPK
    {
        public int Id { get; set; }

        //new public Expression<Func<TImplement, bool>> ExpressionMatchPk<TImplement>() => (arg) => (arg as IId).Id == this.Id;
        //public Expression<Func<TImplement, bool>> ExpressionMatchPk<TImplement>(int id) => (arg) => (arg as IId).Id == Id;
        //Expression<Func<TImplement, bool>> IPK.ExpressionMatchPk<TImplement>() => (arg) => (arg as IId).Id == this.Id;
        //public bool MatchPk(IId x) => x.Id == this.Id;
        //bool IPK.MatchPk<TImplement>(TImplement x) => this.MatchPk(x as IId);

        new bool IsNew() => Id == 0;
        bool IPK.IsNew() => this.IsNew();
    }

    public interface IObjectId : IPK
    {
        public ObjectId _id { get; set; }

        //new public Expression<Func<TImplement, bool>> ExpressionMatchPk<TImplement>() => (arg) => (arg as IObjectId).id == this.id;
        //public Expression<Func<TImplement, bool>> ExpressionMatchPk<TImplement>(ObjectId id) => (arg) => (arg as IObjectId).id == id;        
        //Expression<Func<TImplement, bool>> IPK.ExpressionMatchPk<TImplement>() => (arg) => (arg as IObjectId).id == this.id;        
        //public bool MatchPk(IObjectId x) => x.id.Equals(this.id);
        //bool IPK.MatchPk<TImplement>(TImplement x) => this.MatchPk(x as IObjectId);

        new bool IsNew() => _id == ObjectId.Empty;
        bool IPK.IsNew() => this.IsNew();
    }
}
