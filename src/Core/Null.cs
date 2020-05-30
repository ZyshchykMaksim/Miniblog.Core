namespace Miniblog.Core.Core
{
    using System;
    using System.Dynamic;
    using ImpromptuInterface;

    public sealed class Null<T> : DynamicObject where T : class
    {
        public static T Instance
        {
            get
            {
                if (!typeof(T).IsInterface)
                {
                    throw new ArgumentException("must be interface!");
                }

                return new Null<T>().ActLike<T>();
            }
        }


        /// <inheritdoc/>
        public override bool TryInvokeMember(
            InvokeMemberBinder binder,
            object[] args,
            out object result)
        {
            result = Activator.CreateInstance(binder.ReturnType);

            return true;
        }
    }
}
