import { IProductOrderByCreateDTO } from "./IProductOrderByCreateDTO";

export interface IOrderByCreateDTO{
    createdUserId:number,
    status:boolean,
    customerId:number,
    totalPrice:number,
    dateOfReceipt:Date,
    deliveryStatus:string,
    deliveryNotes:string,
    productOrders:IProductOrderByCreateDTO[]
}