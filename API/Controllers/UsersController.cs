using System.Collections.Generic;
using System.Threading.Tasks;
using API.DataTransferObj;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

//TODO: Get more descriptive error results rather then just a bool.

namespace API.Controllers
{
    [Authorize]
	public class UsersController : ApiBaseController
	{
        private readonly IUserRepository _UserRepository;

		public UsersController(IUserRepository userRepository, IMapper mapper)
		{
            this._UserRepository = userRepository;
		}

        //api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsersDetailDTO>>> GetUsers(){
            return Ok(await _UserRepository.GetUsersDTOAsync());
        }
        //api/users/1
        [HttpGet("{username}")]
        public async Task<ActionResult<UsersDetailDTO>> GetUser(string username){
            return await _UserRepository.GetUserDTOByUsernameAsync(username);
        }

        //------------- user cart -------------//
        [HttpPut("{username}/cart")]
        public async Task<ActionResult<ControllerBase>> addItemToCart(string username, [FromQuery] int id){
            if(await _UserRepository.AddItemForUserCartAsync(username,id)) return Ok();
            //else
            return BadRequest("Something went wrong while trying to add item to cart.");
        }
        [HttpGet("{username}/cart")]
        public async Task<ActionResult<IEnumerable<CartDTO>>> getUserCart(string username){
            return Ok(await _UserRepository.GetUserCartDTOAsync(username));
        }
        [HttpDelete("{username}/cart")]
        public async Task<ActionResult<ControllerBase>> RemoveItemFromUserCartAsync(string username, [FromQuery] int id){
            if(await _UserRepository.RemoveItemFromUserCartAsync(username,id)) return Ok();
            //else
            return BadRequest("Something went wrong while trying to remove item from cart.");
        }
        
        //------------- user items -------------//
        [HttpPost("{username}/items")]
        public async Task<ActionResult<ControllerBase>> addItemForSale(string username, [FromBody] ItemDTO itemDTO){
            if(await _UserRepository.AddItemForUserAsync(username,itemDTO)) return Ok();
            //else
            return BadRequest("Something went wrong while trying to add the item.");
        }
        [HttpGet("{username}/items")]
        public async Task<ActionResult<IEnumerable<CartDTO>>> getUserItems(string username){
            return Ok(await _UserRepository.GetUserItemsDTOAsync(username));
        }
        [HttpDelete("{username}/items")]
        public async Task<ActionResult<ControllerBase>> RemoveItemFromUser(string username, [FromQuery] int id){
            if(await this._UserRepository.RemoveItemFromUser(username,id)) return Ok();
            //else
            return BadRequest("Failed to remove item.");
        }

        //------------- user transactions -------------// probably dont need the post (adding transactiosn should be done server side)
        [HttpPost("{username}/transactions")]
        public async Task<ActionResult<ControllerBase>> addNewTransaction(string username, [FromBody] TransactionDTO transactionDTO){
            if(await _UserRepository.AddNewUserTransactionAsync(username,transactionDTO)) return Ok();
            //else
            return BadRequest("Something went wrong while adding the transaction.");
        }
        [HttpGet("{username}/transactions")]
        public async Task<ActionResult<IEnumerable<TransactionDTO>>> getUserTransactions(string username){
            return Ok(await _UserRepository.GetUserTransactionsDTOAsync(username));
        }
        [HttpDelete("{username}/transactions")]
        public async Task<ActionResult<ControllerBase>> deleteTransaction(string username,[FromQuery] int id){
            if(await this._UserRepository.RemoveUserTransactionAsync(username,id)) return Ok();
            //else
            return BadRequest("Failed to remove transaction");
        }
	}
}
