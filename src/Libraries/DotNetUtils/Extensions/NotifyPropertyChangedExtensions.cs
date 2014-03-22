// Copyright 2012-2014 Andrew C. Dvorak
//
// This file is part of BDHero.
//
// BDHero is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// BDHero is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with BDHero.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace DotNetUtils.Extensions
{
    /// <summary>
    /// Automatic <see cref="INotifyPropertyChanged"/> extensions.
    /// </summary>
    /// <example>
    /// <code>
    /// public class Employee : INotifyPropertyChanged
    /// {
    ///     public event PropertyChangedEventHandler PropertyChanged;
    ///     private string _firstName;
    ///     public string FirstName
    ///     {
    ///         get { return this._firstName; }
    ///         set
    ///         {
    ///             this._firstName = value;
    ///             this.PropertyChanged.Notify(()=>this.FirstName);
    ///         }
    ///     }
    /// }
    /// 
    /// private void firstName_PropertyChanged(Employee sender)
    /// {
    ///     Console.WriteLine(sender.FirstName);
    /// }
    /// 
    /// employee = new Employee();
    /// employee.SubscribeToChange(() => employee.FirstName, firstName_PropertyChanged);
    /// </code>
    /// </example>
    /// <seealso cref="http://stackoverflow.com/a/527840/467582"/>
    public static class NotifyPropertyChangedExtensions
    {
        #region Delegates

        /// <summary>
        /// A property changed handler without the property name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TSender"></typeparam>
        /// <param name="sender">The object that raised the event.</param>
        public delegate void PropertyChangedHandler<TSender>(TSender sender);

        #endregion

        /// <summary>
        /// Notifies listeners about a change.
        /// </summary>
        /// <param name="eventHandler">The event to raise.</param>
        /// <param name="property">The property that changed.</param>
        public static void Notify(this PropertyChangedEventHandler eventHandler, Expression<Func<object>> property)
        {
            // Check for null
            if (eventHandler == null)
                return;

            // Get property name
            var lambda = property as LambdaExpression;
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = lambda.Body as UnaryExpression;
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                memberExpression = lambda.Body as MemberExpression;
            }

            ConstantExpression constantExpression;
            if (memberExpression.Expression is UnaryExpression)
            {
                var unaryExpression = memberExpression.Expression as UnaryExpression;
                constantExpression = unaryExpression.Operand as ConstantExpression;
            }
            else
            {
                constantExpression = memberExpression.Expression as ConstantExpression;
            }

            var propertyInfo = memberExpression.Member as PropertyInfo;

            // Invoke event
            foreach (Delegate del in eventHandler.GetInvocationList())
            {
                del.DynamicInvoke(new[]
                    {
                        constantExpression.Value, new PropertyChangedEventArgs(propertyInfo.Name)
                    });
            }
        }


        /// <summary>
        /// Subscribe to changes in an object implementing INotifiyPropertyChanged.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectThatNotifies">The object you are interested in.</param>
        /// <param name="property">The property you are interested in.</param>
        /// <param name="handler">The delegate that will handle the event.</param>
        public static void SubscribeToChange<T>(this T objectThatNotifies, Expression<Func<object>> property,
                                                PropertyChangedHandler<T> handler) where T : INotifyPropertyChanged
        {
            // Add a new PropertyChangedEventHandler
            objectThatNotifies.PropertyChanged += (s, e) =>
                {
                    // Get name of Property
                    var lambda = property as LambdaExpression;
                    MemberExpression memberExpression;
                    if (lambda.Body is UnaryExpression)
                    {
                        var unaryExpression = lambda.Body as UnaryExpression;
                        memberExpression = unaryExpression.Operand as MemberExpression;
                    }
                    else
                    {
                        memberExpression = lambda.Body as MemberExpression;
                    }
                    var propertyInfo = memberExpression.Member as PropertyInfo;

                    // Notify handler if PropertyName is the one we were interested in
                    if (e.PropertyName.Equals(propertyInfo.Name))
                    {
                        handler(objectThatNotifies);
                    }
                };
        }
    }
}
