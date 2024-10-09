import { IAddress } from "./IAddress";
import { IUser } from "./IUser";

export interface ICustomer{
    createdUserId:number,
    createdDate:Date,
    lastUpdatedUserId:number,
    lastUpdatedDate:Date,
    status:boolean,
    isDeleted:boolean,
    id:number,
    customerCode:string,
    user:IUser,
    userId:number,
    address:IAddress,
    addressId:number
}