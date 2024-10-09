import { IUser } from "../IUser";
import { IAddressByUpdateDTO } from "./IAddressByUpdateDTO";

export interface ICustomerByUpdateDTO{
    lastUpdatedUserId:number,
    status:boolean,
    isDeleted:boolean,
    id:number,
    customerCode:string,
    user:IUser,
    userId:number,
    address:IAddressByUpdateDTO,
    addressId:number
}