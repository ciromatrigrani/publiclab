import { useState, useEffect } from 'react';

function PersonalInfoPage() {

  const [myPersonalInfoList, setMyPersonalInfoList] = useState('')//[] as Array<AuthDataReponse>)

  useEffect(() => {
    loadPage();
  });

  const loadPage = () => {

    async function tryFetch() {
      const queryParameters = new URLSearchParams(window.location.search)
      const token = queryParameters.get('token');
      console.log("Got token : " + token)
      const res = await fetch('https://localhost:7234/api/v1/auth', {
        method: 'GET',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json',
          'Authorization': 'Bearer ' + token
        }
      });
      if (res.ok) {
        setMyPersonalInfoList(await res.json())
      } else {
        console.log(res);
      }
    }
    tryFetch();
  }

  return (
    <div className="PersonalInfoPage">
      <h2>Personal Info</h2>
      <div>Username | Password | Login Id</div>
      {
        myPersonalInfoList && myPersonalInfoList.length > 0 && (
          <>
            {
              myPersonalInfoList.map(pie => (
                <div key={pie.loginId}>{pie.username} | {pie.password} | {pie.loginId}</div>
              ))
            }
          </>
        )
      }
    </div>
  );
}

export default PersonalInfoPage;
