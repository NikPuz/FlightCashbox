//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FlightCashbox.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Distance
    {
        public int Distance1 { get; set; }
        public int idStation_1 { get; set; }
        public int idStation_2 { get; set; }
    
        public virtual Station Station { get; set; }
        public virtual Station Station1 { get; set; }
    }
}
