import { ICustomer } from "./ICustomer";
import { IProductOrder } from "./IProductOrder";

export interface IOrder{
    createdUserId:number,
    createdDate:Date,
    lastUpdatedUserId:number,
    lastUpdatedDate:Date,
    status:boolean,
    isDeleted:boolean,
    id:number,
    customer:ICustomer,
    customerId:number,
    totalPrice:number,
    dateOfReceipt:Date,
    deliveryStatus:string,
    deliveryNotes:string,
    productOrders:IProductOrder[]
}