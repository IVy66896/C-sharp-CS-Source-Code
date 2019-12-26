using System;
using System.IO;
using System.Net;

public abstract class WebRequestState
{
	public int bytesRead;

	public long totalBytes;

	public double progIncrement;

	public Stream streamResponse;

	public byte[] bufferRead;

	public Uri fileURI;

	public string FTPMethod;

	public DateTime transferStart;

	private WebRequest _request;

	private WebResponse _response;

	public virtual WebRequest request
	{
		get
		{
			return null;
		}
		set
		{
			_request = value;
		}
	}

	public virtual WebResponse response
	{
		get
		{
			return null;
		}
		set
		{
			_response = value;
		}
	}

	public WebRequestState(int buffSize)
	{
		bytesRead = 0;
		bufferRead = new byte[buffSize];
		streamResponse = null;
	}
}
