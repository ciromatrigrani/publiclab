import { useState } from 'react';

const LoginForm = () => {

  const [myUsername, setMyUsername] = useState('');
  const [myPassword, setMyPassword] = useState('');

  const submitForm = (e) => {
    e.preventDefault();
    console.log("Initializing Login SSO")
    fetch('https://localhost:7234/api/v1/auth', {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        Username: myUsername,
        Password: myPassword
      })
    })
      .then((res) => {

        async function redirect() {
          if (res.ok)
            window.location.href = "http://localhost:7771?token=" + await res.json();
          else {
            console.log(res);
            alert("ERROR:" + res.status);
          }
        }

        setMyUsername('Registered!');
        setMyPassword('');
        redirect()
      })
      .then((post) => {
        setMyUsername('Registered!');
        setMyPassword('');
      })
      .catch((err) => {
        setMyUsername('Unauthorized!');
        setMyPassword('');
      });
  }

  return (
    <form className="LoginForm" onSubmit={submitForm} >
      <h1>Login Form</h1>
      <div>
        <label htmlFor="myUsername" >Username: </label>
        <input type="text" name="myUsername" id="myUsername" placeholder="Type here the Username" onChange={e => setMyUsername(e.target.value)} />
      </div>
      <div>
        <label htmlFor="myUsername" >Password: </label>
        <input type="password" name="myPassword" placeholder="Type here the passwords" onChange={e => setMyPassword(e.target.value)} />
      </div>
      <div>
        <input type="submit" value="LOGIN" />
      </div>
    </form>
  );
}

export default LoginForm;
